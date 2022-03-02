using App.CQRS.Tasks.Common.Commands.Command;
using App.Services;
using Cayent.Core.CQRS.Commands;
using Cayent.Core.CQRS.Services;
using Data.App.DbContext;
using Data.App.Models.Activities;
using Data.App.Models.Contacts;
using Data.App.Models.Users.UserTasks;
using Data.Constants;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.Common.Extensions;

namespace App.CQRS.Tasks.Common.Commands.Handler
{
    public sealed class TaskCommonCommandHandler :
        ICommandHandler<AddTaskCommand>,
        ICommandHandler<DeleteTaskCommand>,
        ICommandHandler<UpdateTaskItemCommand>

    {
        readonly AppDbContext _dbContext;
        readonly ISequentialGuidGenerator _guidGenerator;
        public TaskCommonCommandHandler(AppDbContext dbContext, ISequentialGuidGenerator guidGenerator)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _guidGenerator = guidGenerator ?? throw new ArgumentNullException(nameof(guidGenerator));
        }

        async Task ICommandHandler<AddTaskCommand>.HandleAsync(AddTaskCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var taskItemNumber = 1;

            var userTask = new UserTask
            {
                UserTaskId = command.UserTaskId,
                Type = command.TaskType,
                Status = Data.Enums.EnumTaskStatus.Todo,
                ContactId = command.ContactId,
                Title = command.Title,
                Description = command.Description,
                UserId = command.UserId,
                RoleId = ApplicationRoles.Member.Id,
                DateAssigned = DateTime.UtcNow.Truncate(),
                DateCreated = DateTime.UtcNow.Truncate(),

                UserTaskItems = command.TaskItems.Select(e => new UserTaskItem
                {
                    UserTaskItemId = _guidGenerator.NewId(),
                    Number = taskItemNumber++,
                    Title = e,
                    IsDone = false,
                }).ToList(),
                
                DateCompleted = command.DateToComplete,
            };

            var activity = CreateNewActivity(command.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Task added.");

            await _dbContext.AddRangeAsync(userTask, activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<DeleteTaskCommand>.HandleAsync(DeleteTaskCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var userTask = await _dbContext.UserTasks.FirstOrDefaultAsync(e => e.UserTaskId == command.UserTaskId);

            userTask.ThrowIfNullOrAlreadyUpdated(command.Token, _guidGenerator.NewId());

            userTask.DateDeleted = DateTime.UtcNow;

            var activity = CreateNewActivity(userTask.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Task deleted.");

            await _dbContext.AddAsync(activity);

            await _dbContext.SaveChangesAsync();
        }

        async Task ICommandHandler<UpdateTaskItemCommand>.HandleAsync(UpdateTaskItemCommand command, System.Threading.CancellationToken cancellationToken)
        {
            var task = await _dbContext.UserTasks.Include(e => e.UserTaskItems).FirstOrDefaultAsync(e => e.UserTaskId == command.UserTaskId);
            task.ThrowIfNull();

            var taskItem = task.UserTaskItems.FirstOrDefault(e => e.UserTaskItemId == command.UserTaskItemId);
            taskItem.ThrowIfNull();

            taskItem.IsDone = command.IsDone;
            taskItem.DateCompleted = command.IsDone ? DateTime.UtcNow : DateTime.MaxValue;

            if (task.UserTaskItems.All(e => !e.IsDone))
            {
                task.Status = Data.Enums.EnumTaskStatus.Todo;
                task.DateActualCompleted = DateTime.MaxValue;
            }

            if (task.UserTaskItems.Any(e => e.IsDone))
            {
                task.Status = Data.Enums.EnumTaskStatus.InProgress;
                task.DateActualCompleted = DateTime.MaxValue;
            }

            if (task.UserTaskItems.All(e => e.IsDone))
            {
                task.Status = Data.Enums.EnumTaskStatus.Done;
                task.DateActualCompleted = DateTime.UtcNow;
            }

            var activity = CreateNewActivity(task.ContactId, command.UserId, EnumActivityEntityType.Contact, $"Task updated.");

            await _dbContext.AddAsync(activity);

            await _dbContext.SaveChangesAsync();
        }


        #region Helper

        ContactActivity CreateNewActivity(string contactId, string userId, EnumActivityEntityType activityEntityType, string description)
        {
            var activity = new Activity
            {
                ActivityId = _guidGenerator.NewId(),
                ActivityEntityType = activityEntityType,
                EntityId = contactId,
                UserId = userId,
                Description = description,
                DateCreated = DateTime.UtcNow
            };


            var contactActivity = new ContactActivity
            {
                ContactId = contactId,
                Activity = activity,
            };

            return contactActivity;
        }

        #endregion
    }
}
