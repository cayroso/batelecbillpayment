using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRazor.ViewModels.Accounts
{
    public class ViewConsumerInfo
    {
        public string UserId { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string AccountNumber { get; set; }
        public string MeterNumber { get; set; }
        public string ConsumerType { get; set; }
        public string Address { get; set; }

        public string FirstLastName { get; set; }

        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }

        public bool IsLocked { get; set; }
    }
}
