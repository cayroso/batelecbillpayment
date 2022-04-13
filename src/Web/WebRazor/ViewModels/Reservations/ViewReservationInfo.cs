using Cayent.Core.Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebRazor.ViewModels.Reservations
{
    public class ViewReservationInfo
    {
        public string ReservationId { get; set; }

        public string BranchId { get; set; }
        public string BranchName { get; set; }

        public string AccountId { get; set; }
        public string AccountName { get; set; }


        DateTime _dateReservation;
        public DateTime DateReservation
        {
            get => _dateReservation;
            set => _dateReservation = value.Truncate();
        }
    }
}
