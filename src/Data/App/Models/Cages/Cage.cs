
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Cayent.Core.Common.Extensions;
using Data.App.Models.Breedings;
using Data.App.Models.Farms;
using Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Data.App.Models.Cages
{
    public class Cage
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string CageId { get; set; }

        public string FarmId { get; set; }
        public virtual Farm Farm { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        //public EnumPenType PenType { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<Breeding> Breedings { get; set; } = new List<Breeding>();
    }


    public static class CageExtension
    {
        public static void ThrowIfNull(this Cage me)
        {
            if (me == null)
                throw new ApplicationException("Pen record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Cage me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Pen record already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class CageConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Cage>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Cage> b)
        {
            b.ToTable("Cage");
            b.HasKey(e => e.CageId);

            b.Property(e => e.CageId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.FarmId).HasMaxLength(KeyMaxLength).IsRequired();
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
