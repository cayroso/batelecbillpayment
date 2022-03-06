using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Billing
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
