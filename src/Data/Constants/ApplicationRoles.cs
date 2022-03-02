using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Constants
{
    public sealed class ApplicationRoles
    {
        private ApplicationRoles(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }

        public const string SystemRoleName = "System";
        public static ApplicationRoles System = new ApplicationRoles(SystemRoleName.ToLower(), SystemRoleName);

        public const string AdministratorRoleName = "Administrator";
        public static ApplicationRoles Administrator = new ApplicationRoles(AdministratorRoleName.ToLower(), AdministratorRoleName);

        //public const string SalesManagerRoleName = "SalesManager";
        //public static ApplicationRoles SalesManager = new ApplicationRoles(SalesManagerRoleName.ToLower(), SalesManagerRoleName);

        public const string ManagerRoleName = "Manager";
        public static ApplicationRoles Manager = new ApplicationRoles(ManagerRoleName.ToLower(), ManagerRoleName);

        public const string MemberRoleName = "Member";
        public static ApplicationRoles Member = new ApplicationRoles(MemberRoleName.ToLower(), MemberRoleName);

        public static List<ApplicationRoles> Items
        {
            get
            {
                return new List<ApplicationRoles>
                {
                    System,
                    Administrator,
                    Manager,
                    Member
                };
            }
        }
    }
}
