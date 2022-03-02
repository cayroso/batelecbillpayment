using Data.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Contacts
{
    public static class ContactExtension
    {
        public static void ThrowIfNull(this Contact me)
        {
            if (me == null)
                throw new ApplicationException("Not found.");
        }
        public static void ThrowIfNullOrAlreadyUpdated(this Contact me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Already updated by another user.");

            me.ConcurrencyToken = newToken;
        }

        public static ContactAudit NewAudit(this Contact me, EnumAuditAction action, string userId, DateTime dateLog)
        {
            var audit = new ContactAudit
            {
                AuditAction = action,
                AuditUserId = userId,
                DateLog = dateLog, 
                EntityId = me.ContactId,
                Address = me.Address,
                GeoX = me.GeoX,
                GeoY = me.GeoY,
                Industry = me.Industry,
                Rating = me.Rating,
                ReferralSource = me.ReferralSource,
                Salutation = me.Salutation,
                Status = me.Status,
                Title = me.Title,
                Website = me.Website,
                AnnualRevenue = me.AnnualRevenue,
                AssignedToId = me.AssignedToId,
                BusinessPhone = me.BusinessPhone,
                Company = me.Company,
                CreatedById = me.CreatedById,
                DateCreated = me.DateCreated,
                DateOfInitialContact = me.DateOfInitialContact,
                DateUpdated = me.DateUpdated,
                Email = me.Email,
                Fax = me.Fax,
                FirstName = me.FirstName,
                MiddleName = me.MiddleName,
                LastName = me.LastName,
                HomePhone = me.HomePhone,
                MobilePhone = me.MobilePhone,

            };

            return audit;
        }

        public static Contact Clone(this Contact me)
        {
            var foo = JsonConvert.SerializeObject(me);

            var bar = JsonConvert.DeserializeObject<Contact>(foo);

            return bar;
        }

        public static bool HasChanges(this Contact me, Contact other)
        {
            var foo = JsonConvert.SerializeObject(me.Clone());
            var bar = JsonConvert.SerializeObject(other.Clone());

            return foo != bar;
        }
    }
}
