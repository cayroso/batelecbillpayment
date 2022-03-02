using Data.App.Models.Pens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cayent.Core.Common.Extensions;
using Data.App.Models.Rabbits;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Data.App.Models.Cages;
using Data.App.Models.Farms;
using Data.App.Models.Seasons;

namespace Data.App.Models.Breedings
{
    public class Breeding
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BreedingId { get; set; }

        public string FarmId { get; set; }
        public virtual Farm Farm { get; set; }

        public string SeasonId { get; set; }
        public virtual Season Season { get; set; }

        public string CageId { get; set; }
        public virtual Cage Cage { get; set; }

        public bool BreedingSuccessful { get; set; }

        public int Alive { get; set; }
        public int Dead { get; set; }

        public int Bucks { get; set; }
        public int Does { get; set; }

        public string Notes { get; set; }

        public bool Active { get; set; }

        DateTime _breedingDate;
        public DateTime BreedingDate
        {
            get => _breedingDate.AsUtc();
            set => _breedingDate = value.Truncate();
        }

        DateTime _birthDate = DateTime.MaxValue;
        public DateTime BirthDate
        {
            get => _birthDate.AsUtc();
            set => _birthDate = value.Truncate();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<BreedingParent> Parents { get; set; } = new List<BreedingParent>();
        public virtual ICollection<Rabbit> Children { get; set; } = new List<Rabbit>();
        public virtual ICollection<BreedingMortality> BreedingMortalities { get; set; } = new List<BreedingMortality>();

    }



    public static class BreedingExtension
    {
        public static void ThrowIfNull(this Breeding me)
        {
            if (me == null)
                throw new ApplicationException("Breeding record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Breeding me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Breeding record already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class BreedingConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Breeding>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Breeding> b)
        {
            b.ToTable("Breeding");
            b.HasKey(e => e.BreedingId);

            b.Property(e => e.BreedingId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.FarmId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.SeasonId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.CageId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Notes).HasMaxLength(NoteMaxLength);

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.Parents)
                .WithOne(d => d.Breeding)
                .HasForeignKey(d => d.BreedingId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(e => e.Children)
                .WithOne(d => d.Breeding)
                .HasForeignKey(d => d.BreedingId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
