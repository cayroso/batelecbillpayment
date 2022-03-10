using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Billing
{    
    public class AddBillingInfo
    {

        [Required]
        public string AccountId { get; set; }
        public double BillingAmount { get; set; }
        [Required]
        public string BillingNumber { get; set; }
        [Required]
        public string BillingMonth { get; set; }
        [Required]
        public string BillingYear { get; set; }

        public DateTime ReadingDate { get; set; }
        public DateTime BillingDateStart { get; set; }
        public DateTime BillingDateEnd { get; set; }

        public double PresentReading { get; set; }
        public double PreviousReading { get; set; }
        public double Multiplier { get; set; }
        public double KilloWattHourUsed { get; set; }        
        public DateTime BillingDateDue { get; set; }
        [Required]
        public string Reader { get; set; }        
    }
}
