using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Billing
{    
    public class Billing
    {
        public string BillingId { get; set; }
        public string GCashSourceResourceId { get; set; }
        public string GCashCheckoutUrl { get; set; }
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
        public string Token { get; set; }

        public SourceResource Resource { get; set; }
        public PaymentResource Payment { get; set; }
    }

    public class SourceResource
    {
        public double Amount { get; set; }
        public string CheckoutUrl { get; set; }
        //public double NetAmount { get; set; }
    }

    public class PaymentResource
    {
        public string GcashPaymentId { get; set; }
        public double Amount { get; set; }
        public string Description { get; set; }
        public double Fee { get; set; }
        public double NetAmount { get; set; }
    }
}
