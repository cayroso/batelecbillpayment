
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Cayent.Core.Common.Extensions;
using Data.App.Models.Breedings;
using Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.App.Models.Pens
{
    public class Pen
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string PenId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        //public EnumPenType PenType { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<Breeding> Breedings { get; set; } = new List<Breeding>();
    }


    public static class PenExtension
    {
        public static void ThrowIfNull(this Pen me)
        {
            if (me == null)
                throw new ApplicationException("Pen record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Pen me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Pen record already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class PenConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Pen>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Pen> b)
        {
            b.ToTable("Pen");
            b.HasKey(e => e.PenId);

            b.Property(e => e.PenId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();
            b.Property(e => e.Description).HasMaxLength(DescMaxLength);

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            //b.HasMany(e => e.Breedings)
            //    .WithOne(d => d.Pen)
            //    .HasForeignKey(d => d.PenId);
        }
    }
}
