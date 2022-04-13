using System.ComponentModel.DataAnnotations;

namespace WebRazor.ViewModels.Billing
{
    public class AddBillingInfo
    {

        [Required]
        public string AccountId { get; set; }
        public double Amount { get; set; }
        [Required]
        public string Number { get; set; }
        [Required]
        public string Month { get; set; }
        [Required]
        public string Year { get; set; }

        public DateTime ReadingDate { get; set; }
        public DateTime DateStart { get; set; }
        public DateTime DateEnd { get; set; }

        public double PresentReading { get; set; }
        public double PreviousReading { get; set; }
        public double Multiplier { get; set; }
        public double KilloWattHourUsed { get; set; }        
        public DateTime DateDue { get; set; }
        [Required]
        public string Reader { get; set; }        
    }
}
