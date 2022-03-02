using Data.App.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Teams
{
    public class TeamMember
    {
        public string TeamId { get; set; }
        public virtual Team Team { get; set; }

        public string MemberId { get; set; }
        public virtual User Member { get; set; }
    }
}
