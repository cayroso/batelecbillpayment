using Cayent.Core.Common.Extensions;
using Data.App.Models.Contacts;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users.UserTasks
{
    public abstract class UserTaskBase
    {
        public EnumTaskType Type { get; set; } = EnumTaskType.Unknown;

        public EnumTaskStatus Status { get; set; } = EnumTaskStatus.Unknown;

        public string RoleId { get; set; }
        public virtual Role Role { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }


        DateTime _dateCreated = DateTime.UtcNow.Truncate();
        public DateTime DateCreated
        {
            get => _dateCreated.AsUtc();
            set => _dateCreated = value.Truncate();
        }

        DateTime _dateAssigned = DateTime.MaxValue;
        public DateTime DateAssigned
        {
            get => _dateAssigned.AsUtc();
            set => _dateAssigned = value.Truncate();
        }

        DateTime _dateCompleted = DateTime.MaxValue;
        public DateTime DateCompleted
        {
            get => _dateCompleted.AsUtc();
            set => _dateCompleted = value.Truncate();
        }

        DateTime _dateActualCompleted = DateTime.MaxValue;
        public DateTime DateActualCompleted
        {
            get => _dateActualCompleted.AsUtc();
            set => _dateActualCompleted = value.Truncate();
        }

        DateTime _dateDeleted = DateTime.MaxValue.Truncate();
        public DateTime DateDeleted
        {
            get => _dateDeleted.AsUtc();
            set => _dateDeleted = value.Truncate();
        }
    }
    public class UserTask : UserTaskBase
    {
        public string UserTaskId { get; set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<UserTaskItem> UserTaskItems { get; set; } = new List<UserTaskItem>();
        public virtual ICollection<UserTaskAudit> Audit { get; set; } = new List<UserTaskAudit>();

    }

    public class UserTaskAudit : UserTaskBase
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string AuditId { get; set; }
        public EnumAuditAction AuditAction { get; set; }

        public string AuditUserId { get; set; }
        public virtual User AuditUser { get; set; }

        DateTime _dateLog = DateTime.Now.Truncate().AsUtc();
        public DateTime DateLog
        {
            get => _dateLog.AsUtc();
            set => _dateLog = value.Truncate();
        }

        public string EntityId { get; set; }
        public virtual UserTask Entity { get; set; }
    }


}
