using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Areas.System.Pages.GcashWebhooks
{
    public class ViewModel : Web.Code.BasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }
        public void OnGet()
        {
        }
    }
}
