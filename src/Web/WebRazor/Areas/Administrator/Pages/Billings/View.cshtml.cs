using Microsoft.AspNetCore.Mvc;

namespace WebRazor.Areas.Administrator.Pages.Billings
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
