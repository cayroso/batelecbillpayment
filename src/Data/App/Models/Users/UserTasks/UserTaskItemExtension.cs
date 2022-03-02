using Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users.UserTasks
{
    public static class UserTaskItemExtension
    {
        public static void ThrowIfNull(this UserTaskItem me)
        {
            if (me == null)
                throw new ApplicationException("User Task Item not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this UserTaskItem me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("User Task already updated by another user.");

            me.ConcurrencyToken = newToken;
        }

        public static UserTaskItemAudit NewAudit(this UserTaskItem me, EnumAuditAction action, string userId, DateTime dateLog)
        {
            var audit = new UserTaskItemAudit
            {
                AuditAction = action,
                AuditUserId = userId,
                DateLog = dateLog,
                EntityId = me.UserTaskId,

                DateCompleted = me.DateCompleted,
                DateDeleted = me.DateDeleted,
                Title = me.Title,
                Number = me.Number,
                IsDone = me.IsDone,
                UserTaskId = me.UserTaskId,
            };

            return audit;
        }

        public static UserTaskItem Clone(this UserTaskItem me)
        {
            var foo = JsonConvert.SerializeObject(me);

            var bar = JsonConvert.DeserializeObject<UserTaskItem>(foo);

            return bar;
        }

        public static bool HasChanges(this UserTaskItem me, UserTaskItem other)
        {
            var foo = JsonConvert.SerializeObject(me.Clone());
            var bar = JsonConvert.SerializeObject(other.Clone());

            return foo != bar;
        }
    }
}
