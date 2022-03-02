using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;

namespace App.CQRS.Tasks.Common.Commands.Command
{
    public sealed class AddTaskCommand : AbstractCommand
    {
        public string UserTaskId { get; }
        public string ContactId { get; }
        public EnumTaskType TaskType { get; }
        public string Title { get; }
        public string Description { get; }
        public DateTime DateToComplete { get; }
        public IEnumerable<string> TaskItems { get; }

        public AddTaskCommand(string correlationId, string tenantId, string userId,
            string userTaskId, string contactId, EnumTaskType taskType, string title, string description, DateTime dateToComplete, IEnumerable<string> taskItems)
            : base(correlationId, tenantId, userId)
        {
            UserTaskId = userTaskId;
            ContactId = contactId;
            TaskType = taskType;
            Title = title;
            Description = description;
            DateToComplete = dateToComplete;
            TaskItems = taskItems;
        }
    }
}
