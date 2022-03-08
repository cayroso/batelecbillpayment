using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Billing
{    
    public class AddBillingInfo
    {
        
        public string AccountId { get; set; }
        public double BillingAmount { get; set; }
        public string BillingNumber { get; set; }
        public string BillingMonth { get; set; }
        public string BillingYear { get; set; }

        public DateTime ReadingDate { get; set; }
        public DateTime BillingDateStart { get; set; }
        public DateTime BillingDateEnd { get; set; }

        public double PresentReading { get; set; }
        public double PreviousReading { get; set; }
        public double Multiplier { get; set; }
        public double KilloWattHourUsed { get; set; }        
        public DateTime BillingDateDue { get; set; }

        public string Reader { get; set; }        
    }
}
