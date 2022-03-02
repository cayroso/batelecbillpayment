using Cayent.Core.CQRS.Queries;
using Cayent.Core.Common.Extensions;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Tasks.Common.Queries.Query
{
    public sealed class GetTaskByIdQuery : AbstractQuery<GetTaskByIdQuery.Task>
    {
        public string TaskId { get; }

        public GetTaskByIdQuery(string correlationId, string tenantId, string userId, string taskId)
            : base(correlationId, tenantId, userId)
        {
            TaskId = taskId;
        }

        public class Task
        {
            public string TaskId { get; set; }

            public Contact Contact { get; set; }

            public EnumTaskType TaskType { get; set; }
            public string TaskTypeText => TaskType.ToString();

            public EnumTaskStatus TaskStatus { get; set; }
            public string TaskStatusText => TaskStatus.ToString();

            public string Title { get; set; }
            public string Description { get; set; }

            DateTime _dateCreated;
            public DateTime DateCreated
            {
                get => _dateCreated;
                set => _dateCreated = value.AsUtc();
            }

            DateTime _dateAssigned;
            public DateTime DateAssigned
            {
                get => _dateAssigned;
                set => _dateAssigned = value.AsUtc();
            }

            DateTime _dateCompleted;
            public DateTime DateCompleted
            {
                get => _dateCompleted;
                set => _dateCompleted = value.AsUtc();
            }

            DateTime _dateActualCompleted;
            public DateTime DateActualCompleted
            {
                get => _dateActualCompleted;
                set => _dateActualCompleted = value.AsUtc();
            }

            DateTime _dateDeleted;
            public DateTime DateDeleted
            {
                get => _dateDeleted;
                set => _dateDeleted = value.AsUtc();
            }

            public bool IsDeleted => _dateDeleted < DateTime.UtcNow;

            public string Token { get; set; }

            public IEnumerable<TaskItem> TaskItems { get; set; }
        }

        public class Contact
        {
            public string ContactId { get; set; }
            
            public string StatusText { get; set; }

            public int Rating { get; set; }
            
            public string FirstName { get; set; }
            public string MiddleName { get; set; }
            public string LastName { get; set; }
        }

        public class TaskItem
        {
            public string TaskItemId { get; set; }
            public string Title { get; set; }
            public bool IsDone { get; set; } = false;

            DateTime _dateCompleted;
            public DateTime DateCompleted
            {
                get => _dateCompleted;
                set => _dateCompleted = value.AsUtc();
            }

            public string Token { get; set; }

        }
    }
}
