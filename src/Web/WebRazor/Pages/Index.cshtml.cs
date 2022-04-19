using Data.Constants;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> OnGet()
        {
            //var isSystem = User.IsInRole(ApplicationRoles.System.Name);
            //var isAdmin = User.IsInRole(ApplicationRoles.Administrator.Name);
            //var isConsumer = User.IsInRole(ApplicationRoles.Consumer.Name);

            //var onlySystem = isSystem && !isAdmin && !isConsumer;
            //var onlyAdmin = !isSystem && isAdmin && !isConsumer;
            //var onlyConsumer = !isSystem && !isAdmin && isConsumer;

            //if (onlySystem)
            //    return Redirect("/system");
            //else if(onlyAdmin)
            //    return Redirect("/administrator");
            //else if(onlyConsumer)
            //    return Redirect("/consumer");

            return Page();
        }
    }
}