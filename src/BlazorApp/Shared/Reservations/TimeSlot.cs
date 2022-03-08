using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Reservations
{
    public class TimeSlot
    {
        public string Slot { get; set; }
        public bool Reserved { get; set; }
    }
}
