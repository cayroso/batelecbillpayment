using Microsoft.AspNetCore.Mvc;

namespace WebRazor.Areas.Consumer.Pages.Billings
{
    public class CheckoutModel : Web.Code.BasePageModel
    {
        [BindProperty(SupportsGet = true)]
        public string Id { get; set; }

        public void OnGet()
        {
        }
    }
}
