
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users
{
    public class UserRole
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }

        public string RoleId { get; set; }
        public virtual Role Role { get; set; }

    }
}
