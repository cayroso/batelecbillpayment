using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
