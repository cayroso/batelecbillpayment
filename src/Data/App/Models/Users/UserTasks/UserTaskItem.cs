using Cayent.Core.Common.Extensions;
using Data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users.UserTasks
{
    public abstract class UserTaskItemBase
    {

        public string UserTaskId { get; set; }
        public virtual UserTask UserTask { get; set; }
        public int Number { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; } = false;

        DateTime _dateCompleted = DateTime.MaxValue;
        public DateTime DateCompleted
        {
            get => _dateCompleted.AsUtc();
            set => _dateCompleted = value.Truncate();
        }

        DateTime _dateDeleted = DateTime.MaxValue.Truncate();
        public DateTime DateDeleted
        {
            get => _dateDeleted.AsUtc();
            set => _dateDeleted = value.Truncate();
        }
    }

    public class UserTaskItem : UserTaskItemBase
    {
        public string UserTaskItemId { get; set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<UserTaskItemAudit> Audit { get; set; } = new List<UserTaskItemAudit>();

    }

    public class UserTaskItemAudit : UserTaskItemBase
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
        public virtual UserTaskItem Entity { get; set; }
    }
}
