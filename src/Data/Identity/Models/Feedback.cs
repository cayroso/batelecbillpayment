using Cayent.Core.Common.Extensions;
using Data.Enums;
using Data.Identity.Models.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.Identity.Models
{
    public class Feedback
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string FeedbackId { get; set; }

        public string UserId { get; set; }
        public IdentityWebUser User { get; set; }

        public EnumFeedbackCategory FeedbackCategory { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }



        DateTime _dateCreated = DateTime.UtcNow.Truncate();
        public DateTime DateCreated
        {
            get => _dateCreated.AsUtc();
            set => _dateCreated = value.Truncate();
        }

        [ConcurrencyCheck]
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
    }
}
