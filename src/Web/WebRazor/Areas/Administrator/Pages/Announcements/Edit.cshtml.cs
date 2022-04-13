using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Areas.Administrator.Pages.Announcements
{
    public class EditModel : Web.Code.BasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public void OnGet()
        {
        }
    }
}
