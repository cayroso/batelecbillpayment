
using Data.App.Models.Rabbits.Activities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.App.Models.Rabbits.Supplies
{
    public class Feed
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string FeedId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<Activity> Feedings { get; set; } = new List<Activity>();
    }

    public class FeedConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Feed>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Feed> b)
        {
            b.ToTable("Feed");
            b.HasKey(e => e.FeedId);

            b.Property(e => e.FeedId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();
            b.Property(e => e.Description).HasMaxLength(DescMaxLength).IsRequired();
            b.Property(e => e.Notes).HasMaxLength(NoteMaxLength).IsRequired();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.Feedings)
                .WithOne(d => d.Feed)
                .HasForeignKey(d => d.FeedId);
        }
    }
}
