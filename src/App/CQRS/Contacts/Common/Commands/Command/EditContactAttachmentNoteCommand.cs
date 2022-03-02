using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.CQRS.Commands;
namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class EditContactAttachmentNoteCommand : AbstractCommand
    {
        public string ContactAttachmentId { get; }
        public string Token { get; }
        public string Title { get; }
        public string Content { get; }
        public EditContactAttachmentNoteCommand(string correlationId, string tenantId, string userId, string contactAttachmentId, string token, string title, string content)
                : base(correlationId, tenantId, userId)
        {
            ContactAttachmentId = contactAttachmentId;
            Token = token;
            Title = title;
            Content = content;
        }
    }
}
