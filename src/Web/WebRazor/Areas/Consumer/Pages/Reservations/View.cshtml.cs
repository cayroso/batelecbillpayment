using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WebRazor.Areas.Consumer.Pages.Reservations
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
