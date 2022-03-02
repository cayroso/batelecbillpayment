using Cayent.Core.Common.Extensions;
using Data.App.Models.FileUploads;
using Data.App.Models.Users;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Contacts
{
    public abstract class ContactAttachmentBase
    {
        public EnumContactAttachmentType AttachmentType { get; set; }

        public string ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string Title { get; set; }

        public string FileUploadId { get; set; }
        public virtual FileUpload FileUpload { get; set; }

        public string Content { get; set; }

        DateTime _dateCreated = DateTime.UtcNow.Truncate();
        public DateTime DateCreated
        {
            get => _dateCreated.AsUtc();
            set => _dateCreated = value.Truncate();
        }

        DateTime _dateUpdated = DateTime.UtcNow.Truncate();
        public DateTime DateUpdated
        {
            get => _dateUpdated.AsUtc();
            set => _dateUpdated = value.Truncate();
        }

        DateTime _dateDeleted = DateTime.MaxValue.Truncate();
        public DateTime DateDeleted
        {
            get => _dateDeleted.AsUtc();
            set => _dateDeleted = value.Truncate();
        }
    }

    public class ContactAttachment: ContactAttachmentBase
    {
        public string ContactAttachmentId { get; set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<ContactAttachmentAudit> Audit { get; set; } = new List<ContactAttachmentAudit>();
    }

    public class ContactAttachmentAudit : ContactAttachmentBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AuditId { get; set; }
        public EnumAuditAction AuditAction { get; set; }

        public string AuditUserId { get; set; }
        public virtual User AuditUser { get; set; }

        DateTime _dateLog = DateTime.UtcNow.Truncate().AsUtc();
        public DateTime DateLog
        {
            get => _dateLog.AsUtc();
            set => _dateLog = value.Truncate();
        }

        public string EntityId { get; set; }
        public virtual ContactAttachment Entity { get; set; }
    }
}
