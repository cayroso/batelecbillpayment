
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.App.Models.Rabbits
{
    public class Bloodline
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BloodlineId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<RabbitBloodline> RabbitBloodlines { get; set; } = new List<RabbitBloodline>();
    }

    public static class BloodlineExtension
    {
        public static void ThrowIfNull(this Bloodline me)
        {
            if (me == null)
                throw new ApplicationException("Bloodline record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Bloodline me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Bloodline recrd already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class BloodlineConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Bloodline>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Bloodline> b)
        {
            b.ToTable("Bloodline");
            b.HasKey(e => e.BloodlineId);

            b.Property(e => e.BloodlineId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(KeyMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();
            b.Property(e => e.Description).HasMaxLength(DescMaxLength);

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.RabbitBloodlines)
                .WithOne(d => d.Bloodline)
                .HasForeignKey(d => d.BloodlineId);
        }
    }
}
