using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRazor.ViewModels.Billing
{    
    public class ViewBillingInfo
    {
        public string BillingId { get; set; }

        public string AccountNumber { get; set; }
        public string AccountName { get; set; }

        public int Status { get; set; }
        public string StatusText { get; set; }
        public string GCashSourceResourceId { get; set; }
        public string GCashCheckoutUrl { get; set; }
        public double Amount { get; set; }
        public string Number { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }

        public DateTime ReadingDate { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public double PresentReading { get; set; }
        public double PreviousReading { get; set; }
        public double Multiplier { get; set; }
        public double KilloWattHourUsed { get; set; }        
        public DateTime DateDue { get; set; }

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
