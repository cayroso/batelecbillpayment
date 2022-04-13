using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Blazor.Shared.Security
{
    public class UserInfo
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public IEnumerable<ClaimInfo> Claims { get; set; }
    }

    public class ClaimInfo
    {
        public string ClaimType { get; set; }
        public string Value { get; set; }
    }
}
