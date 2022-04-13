using System.ComponentModel.DataAnnotations;

namespace WebRazor.ViewModels.Billing
{
    public class AddBillingSourceInfo
    {
        [Required]
        public string BillingId { get; set; }
        [Required]
        public string SourceId { get; set; }
        [Required]
        public string CheckoutUrl { get; set; }
        public double Amount { get; set; }
        public string Status { get; set; }
    }
}
