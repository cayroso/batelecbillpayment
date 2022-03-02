using Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users.UserTasks
{
    public static class UserTaskExtension
    {
        public static void ThrowIfNull(this UserTask me)
        {
            if (me == null)
                throw new ApplicationException("User Task not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this UserTask me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("User Task already updated by another user.");

            me.ConcurrencyToken = newToken;
        }

        public static UserTaskAudit NewAudit(this UserTask me, EnumAuditAction action, string userId, DateTime dateLog)
        {
            var audit = new UserTaskAudit
            {
                AuditAction = action,
                AuditUserId = userId,
                DateLog = dateLog,
                EntityId = me.UserTaskId,

                DateCompleted = me.DateCompleted,
                DateDeleted = me.DateDeleted,
                ContactId = me.ContactId,
                DateActualCompleted = me.DateActualCompleted,
                DateAssigned = me.DateAssigned,
                DateCreated = me.DateCreated,
                Description = me.Description,
                RoleId = me.RoleId,
                Status = me.Status,
                Title = me.Title,
                Type = me.Type,
                UserId = me.UserId,                
            };

            return audit;
        }

        public static UserTask Clone(this UserTask me)
        {
            var foo = JsonConvert.SerializeObject(me);

            var bar = JsonConvert.DeserializeObject<UserTask>(foo);

            return bar;
        }

        public static bool HasChanges(this UserTask me, UserTask other)
        {
            var foo = JsonConvert.SerializeObject(me.Clone());
            var bar = JsonConvert.SerializeObject(other.Clone());

            return foo != bar;
        }
    }
}
