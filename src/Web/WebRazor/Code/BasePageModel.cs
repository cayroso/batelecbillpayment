using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace Web.Code
{
    public abstract class BasePageModel : PageModel
    {
        public string TenantId
        {
            get
            {
                return User.FindFirstValue("TenantId");
            }
        }

        public string TenantName
        {
            get
            {
                return User.FindFirstValue("TenantName");
            }
        }

        public string UserId
        {
            get
            {
                return User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
        }
    }
}
