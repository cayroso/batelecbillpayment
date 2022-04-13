using System.ComponentModel.DataAnnotations;

namespace WebRazor.ViewModels.Reservations
{
    public class AddReservationInfo
    {
        [Required]
        public DateTime DateReservation { get; set; }        
        [Required]
        public string BranchId { get; set; }
    }
}
