using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Users
{
    public class Role
    {
        public string RoleId { get; set; }
        public string Name { get; set; }
        //public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    }
}
