using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;

namespace App.CQRS.Tasks.Common.Commands.Command
{
    public sealed class UpdateTaskItemCommand : AbstractCommand
    {
        public string UserTaskId { get; }
        public string UserTaskItemId { get; }
        public bool IsDone { get; }

        public UpdateTaskItemCommand(string correlationId, string tenantId, string userId, string userTaskId, string userTaskItemId, bool isDone)
            : base(correlationId, tenantId, userId)
        {
            UserTaskId = userTaskId;
            UserTaskItemId = userTaskItemId;
            IsDone = isDone;
        }
    }
}
