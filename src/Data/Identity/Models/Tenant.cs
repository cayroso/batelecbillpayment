
using Data.Identity.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Data.Identity.Models
{
    public class Tenant
    {
        [Required]
        public string TenantId { get; set; }

        public string Name { get; set; }
        public string Host { get; set; }
        public string DatabaseConnectionString { get; set; }

        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public double GeoX { get; set; }
        public double GeoY { get; set; }

        public virtual ICollection<IdentityWebUser> Users { get; set; } = new List<IdentityWebUser>();
    }

    internal static class TenantExtension
    {

        public static void ThrowIfNull(this Tenant me)
        {
            if (me == null)
                throw new ApplicationException("Tenant not found.");
        }
        //public static void ThrowIfNullOrAlreadyUpdated(this Tenant me, string currentToken, string newToken)
        //{
        //    me.ThrowIfNull();

        //    if (string.IsNullOrWhiteSpace(newToken))
        //        throw new ApplicationException("Anti-forgery token not found.");

        //    if (me.ConcurrencyToken != currentToken)
        //        throw new ApplicationException("Bottle already updated by another user.");

        //    me.ConcurrencyToken = newToken;
        //}
    }
}
