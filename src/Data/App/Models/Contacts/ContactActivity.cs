using Data.App.Models.Activities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Contacts
{
    public class ContactActivity
    {
        public string ContactId { get; set; }
        public virtual Contact Contact { get; set; }

        public string ActivityId { get; set; }
        public virtual Activity Activity { get; set; }
    }
}
