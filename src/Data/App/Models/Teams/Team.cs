using Cayent.Core.Common.Extensions;
using Data.App.Models.Chats;
using Data.App.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Teams
{
    public class Team
    {
        public string TeamId { get; set; }

        public virtual Chat Chat { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => _dateCreated = value.Truncate().AsUtc();
        }

        DateTime _dateUpdated;
        public DateTime DateUpdated
        {
            get => _dateUpdated;
            set => _dateUpdated = value.Truncate().AsUtc();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    }

    public static class TeamExtension
    {
        public static void ThrowIfNull(this Team me)
        {
            if (me == null)
                throw new ApplicationException("Not found.");
        }
        public static void ThrowIfNullOrAlreadyUpdated(this Team me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }
}
