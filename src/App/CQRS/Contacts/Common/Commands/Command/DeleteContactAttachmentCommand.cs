using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class DeleteContactAttachmentCommand : AbstractCommand
    {
        public string ContactAttachmentId { get; }
        public string Token { get; }
        public bool Purge { get; }

        public DeleteContactAttachmentCommand(string correlationId, string tenantId, string userId, string contactAttachmentId, string token, bool purge)
                : base(correlationId, tenantId, userId)
        {
            ContactAttachmentId = contactAttachmentId;
            Token = token;
            Purge = purge;
        }
    }
}
