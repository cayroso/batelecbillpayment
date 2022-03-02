
using Data.App.Models.Breedings;
using System;
using System.Collections.Generic;
using System.Text;
using Cayent.Core.Common.Extensions;
using Data.Enums;
using Data.App.Models.Rabbits.Activities;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Data.App.Models.Cages;
using Data.App.Models.Farms;

namespace Data.App.Models.Rabbits
{
    public class Rabbit
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RabbitId { get; set; }
        
        public string FarmId { get; set; }
        public virtual Farm Farm { get; set; }

        public string BreedingId { get; set; }
        public virtual Breeding Breeding { get; set; }

        public string CageId { get; set; }
        public virtual Cage Cage { get; set; }

        public EnumRabbitGender Gender { get; set; }

        public string Name { get; set; }
        
        public string PrimaryImageUrl { get; set; }
        
        public float FertilityRate { get;set; }

        DateTime _dateStart;
        public DateTime DateStart
        {
            get => _dateStart.AsUtc();
            set => _dateStart = value.Truncate();
        }

        DateTime _dateEnd = DateTime.MaxValue;
        public DateTime DateEnd
        {
            get => _dateEnd.AsUtc();
            set => _dateEnd = value.Truncate();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<BreedingParent> OwnedBreedings { get; set; } = new List<BreedingParent>();
        public virtual ICollection<RabbitBloodline> Bloodlines { get; set; } = new List<RabbitBloodline>();
        public virtual ICollection<RabbitImage> RabbitImages { get; set; } = new List<RabbitImage>();
    }

    public static class BirdExtension
    {
        public static void ThrowIfNull(this Rabbit me)
        {
            if (me == null)
                throw new ApplicationException("Rabbit record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Rabbit me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Rabbit record already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class RabbitConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Rabbit>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Rabbit> b)
        {
            b.ToTable("Rabbit");
            b.HasKey(e => e.RabbitId);

            b.Property(e => e.RabbitId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.FarmId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BreedingId).HasMaxLength(KeyMaxLength);
            b.Property(e => e.CageId).HasMaxLength(KeyMaxLength);
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();
           
            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.Bloodlines)
                .WithOne(d => d.Rabbit)
                .HasForeignKey(d => d.RabbitId);

            //b.HasMany(e => e.Activities)
            //    .WithOne(d => d.Bird)
            //    .HasForeignKey(d => d.BirdId);

            b.HasMany(d => d.OwnedBreedings)
                .WithOne(e => e.Rabbit)
                .HasForeignKey(e => e.RabbitId);

            b.HasMany(e => e.RabbitImages)
                .WithOne(d => d.Rabbit)
                .HasForeignKey(d => d.RabbitId);

            //b.HasQueryFilter(e => e.DateEnd > DateTime.UtcNow);
        }
    }
}
