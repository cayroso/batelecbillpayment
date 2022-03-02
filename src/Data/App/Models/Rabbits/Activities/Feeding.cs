
using Data.App.Models.Rabbits.Supplies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.App.Models.Rabbits.Activities
{
    public class Feeding
    {
        public string ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public string FeedId { get; set; }
        public virtual Feed Feed { get; set; }
    }

    public class FeedingConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Feeding>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Feeding> b)
        {
            b.ToTable("Feeding");
            b.HasKey(e => e.ActivityId);

            b.Property(e => e.ActivityId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.FeedId).HasMaxLength(KeyMaxLength).IsRequired();
        }
    }
}
