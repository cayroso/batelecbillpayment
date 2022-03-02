using App.CQRS.Tasks.Common.Queries.Query;
using Cayent.Core.CQRS.Queries;
using Data.App.DbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Tasks.Common.Queries.Handler
{
    public sealed class TaskCommonQueryHandler :
        IQueryHandler<GetTaskByIdQuery, GetTaskByIdQuery.Task>
    {
        readonly AppDbContext _dbContext;
        public TaskCommonQueryHandler(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        async Task<GetTaskByIdQuery.Task> IQueryHandler<GetTaskByIdQuery, GetTaskByIdQuery.Task>.HandleAsync(GetTaskByIdQuery query, System.Threading.CancellationToken cancellationToken)
        {
            var sql = from task in _dbContext.UserTasks.Include(e => e.UserTaskItems).AsNoTracking()

                      where task.UserTaskId == query.TaskId

                      select new GetTaskByIdQuery.Task
                      {
                          TaskId = task.UserTaskId,
                          TaskType = task.Type,
                          TaskStatus = task.Status,

                          Title = task.Title,
                          Description = task.Description,

                          Contact = new GetTaskByIdQuery.Contact
                          {
                              ContactId = task.Contact.ContactId,
                              StatusText = task.Contact.Status.ToString(),
                              Rating = task.Contact.Rating,
                              FirstName = task.Contact.FirstName,
                              MiddleName = task.Contact.MiddleName,
                              LastName = task.Contact.LastName,
                          },

                          DateCreated = task.DateCreated,
                          DateAssigned = task.DateAssigned,
                          DateCompleted = task.DateCompleted,
                          DateActualCompleted = task.DateActualCompleted,
                          DateDeleted = task.DateDeleted,

                          Token = task.ConcurrencyToken,

                          TaskItems = task.UserTaskItems.OrderBy(e => e.Number).Select(e => new GetTaskByIdQuery.TaskItem
                          {
                              TaskItemId = e.UserTaskItemId,
                              Title = e.Title,
                              IsDone = e.IsDone,
                              DateCompleted = e.DateCompleted,
                              Token = e.ConcurrencyToken
                          })
                      };

            var dto = await sql.FirstOrDefaultAsync();

            return dto;
        }
    }
}
