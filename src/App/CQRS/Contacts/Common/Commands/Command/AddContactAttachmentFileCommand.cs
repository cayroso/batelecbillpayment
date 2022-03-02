using Cayent.Core.CQRS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.CQRS.Contacts.Common.Commands.Command
{
    public sealed class AddContactAttachmentFileCommand : AbstractCommand
    {
        public string ContactId { get; }
        public string Token { get; }

        public string ContactAttachmentId { get; }
        public string FileUploadId { get; }
        public string Url { get; }
        public string FileName { get; }
        public string ContentDisposition { get; }
        public string ContentType { get; }
        public byte[] Content { get; }
        public long Length { get; }

        public AddContactAttachmentFileCommand(string correlationId, string tenantId, string userId, string contactId, string token,
            string contactAttachmentId, string fileUploadId, string url, string fileName, string contentDisposition, string contentType, byte[] content, long length)
                : base(correlationId, tenantId, userId)
        {
            ContactId = contactId;
            Token = token;
            ContactAttachmentId = contactAttachmentId;
            FileUploadId = fileUploadId;
            Url = url;
            FileName = fileName;
            ContentDisposition = contentDisposition;
            ContentType = contentType;
            Content = content;
            Length = length;
        }
    }
}
