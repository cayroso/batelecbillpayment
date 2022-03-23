using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Announcements
{
    public class Announcement
    {
        public string AnnouncementId { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        DateTime _dateCreated;
        public DateTime DateCreated
        {
            get => _dateCreated;
            set => _dateCreated = value.Truncate().AsUtc();
        }
    }

    internal class AnnouncementConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Announcement>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Announcement> b)
        {
            b.ToTable("Announcement");
            b.HasKey(e => e.AnnouncementId);

            b.Property(e => e.AnnouncementId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Subject).IsRequired();
            b.Property(e => e.Content).IsRequired();            
        }
    }
}
