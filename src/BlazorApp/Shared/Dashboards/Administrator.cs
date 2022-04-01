using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared.Dashboards
{
    public class Administrator
    {
        public int Announcements { get; set; }
        public int Notifications { get; set; }


        public int TodayReservations {get;set;}
        public int TomorrowReservations { get;set;}
        public int WeekReservations { get;set;}

        public int PastDueDateBillings { get; set; }
        public int TodayDueDateBillings { get; set; }
        public int TomorrowDueDateBillings { get; set; }
        public int WeekDueDateBillings { get; set; }

        
    }
}
