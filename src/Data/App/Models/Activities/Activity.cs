using Cayent.Core.Common.Extensions;
using Data.App.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Activities
{
    public class Activity
    {
        public string ActivityId { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public EnumActivityEntityType ActivityEntityType { get; set; }

        public string EntityId { get; set; }

        public string Description { get; set; }

        DateTime _dateCreated = DateTime.UtcNow.Truncate();
        public DateTime DateCreated
        {
            get => _dateCreated.AsUtc();
            set => _dateCreated = value.Truncate();
        }
    }

    public enum EnumActivityEntityType
    {
        Unknown = 0,
        Contact = 1,
        Document = 2,
        Task = 3,
        TaskItem = 4
    }
}
