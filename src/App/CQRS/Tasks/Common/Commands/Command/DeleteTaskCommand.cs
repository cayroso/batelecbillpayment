using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;

namespace App.CQRS.Tasks.Common.Commands.Command
{
    public sealed class DeleteTaskCommand : AbstractCommand
    {
        public string UserTaskId { get; }
        public string Token { get; }
        public DeleteTaskCommand(string correlationId, string tenantId, string userId, string userTaskId, string token)
            : base(correlationId, tenantId, userId)
        {
            UserTaskId = userTaskId;
            Token = token;
        }
    }
}
