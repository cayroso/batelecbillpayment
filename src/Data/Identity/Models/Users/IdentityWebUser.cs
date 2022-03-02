using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Identity.Models.Users
{
    public class IdentityWebUser : IdentityUser
    {
        public string TenantId { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public virtual ICollection<LoginAudit> LoginAudits { get; set; }
    }
}
