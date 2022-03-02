using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Constants
{
    public sealed class StaffRoles
    {
        private StaffRoles(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Id { get; }
        public string Name { get; }

        public const string ManagerRoleName = "Manager";
        public static StaffRoles Manager = new StaffRoles(ManagerRoleName.ToLower(), ManagerRoleName);

        public const string AssistantRoleName = "Assistant";
        public static StaffRoles Assistant = new StaffRoles(AssistantRoleName.ToLower(), AssistantRoleName);

        public const string FrontlinerRoleName = "Frontliner";
        public static StaffRoles Frontliner = new StaffRoles(FrontlinerRoleName.ToLower(), FrontlinerRoleName);

        public const string DriverRoleName = "Driver";
        public static StaffRoles Driver = new StaffRoles(DriverRoleName.ToLower(), DriverRoleName);

        public const string CustomerRoleName = "Customer";
        public static StaffRoles Customer = new StaffRoles(CustomerRoleName.ToLower(), CustomerRoleName);

        public static List<StaffRoles> Items
        {
            get
            {
                return new List<StaffRoles>
                {
                    Manager,
                    Assistant,
                    Frontliner,
                    Driver,
                    Customer
                };
            }
        }
    }
}
