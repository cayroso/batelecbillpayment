using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class AddContactAttachmentNoteCommand : AbstractCommand
    {
        public string ContactId { get; }
        public string Token { get; }
        public string ContactAttachmentId { get; }
        public string Title { get; }
        public string Content { get; }

        public AddContactAttachmentNoteCommand(string correlationId, string tenantId, string userId, string contactId, string token,
            string contactAttachmentId, string title, string content)
                : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
            Token = token;
            ContactAttachmentId = contactAttachmentId;
            Title = title;
            Content = content;
        }
    }
}
