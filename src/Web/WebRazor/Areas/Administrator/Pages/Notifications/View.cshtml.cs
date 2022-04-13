using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Areas.Administrator.Pages.Notifications
{
    public class ViewModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public void OnGet()
        {
        }
    }
}
