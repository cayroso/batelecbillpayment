using Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Contacts
{
    public static class ContactAttachmentExtension
    {
        public static void ThrowIfNull(this ContactAttachment me)
        {
            if (me == null)
                throw new ApplicationException("Not found.");
        }
        public static void ThrowIfNullOrAlreadyUpdated(this ContactAttachment me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Already updated by another user.");

            me.ConcurrencyToken = newToken;
        }

        public static ContactAttachmentAudit NewAudit(this ContactAttachment me, EnumAuditAction action, string userId)
        {
            var audit = new ContactAttachmentAudit
            {
                EntityId = me.ContactAttachmentId,
                ContactId = me.ContactId,
                AttachmentType = me.AttachmentType,
                Title = me.Title,
                Content = me.Content,
                FileUploadId = me.FileUploadId,
                DateCreated = me.DateCreated,
                DateUpdated = me.DateUpdated,
                DateDeleted = me.DateDeleted,

                AuditAction = action,
                AuditUserId = userId,
                DateLog = DateTime.UtcNow,
            };

            return audit;
        }

        public static ContactAttachment Clone(this ContactAttachment me)
        {
            var temp = new ContactAttachment
            {
                AttachmentType = me.AttachmentType,
                ContactAttachmentId = me.ContactAttachmentId,
                ContactId = me.ContactId,
                Content = me.Content,
                DateCreated = me.DateCreated,
                DateDeleted = me.DateDeleted,
                DateUpdated = me.DateUpdated,
                FileUploadId = me.FileUploadId,
                Title = me.Title,
                ConcurrencyToken = me.ConcurrencyToken
            };

            return temp;

            //var foo = JsonConvert.SerializeObject(me);

            //var bar = JsonConvert.DeserializeObject<ContactAttachment>(foo);

            //return bar;
        }

        public static bool HasChanges(this ContactAttachment me, ContactAttachment other)
        {
            var foo = JsonConvert.SerializeObject(me.Clone());
            var bar = JsonConvert.SerializeObject(other.Clone());

            return foo != bar;
        }
    }
}
