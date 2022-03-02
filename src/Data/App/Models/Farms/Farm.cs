using Data.App.Models.Breedings;
using Data.App.Models.Cages;
using Data.App.Models.Rabbits;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Farms
{
    public class Farm
    {
        public string FarmId { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<Rabbit> Rabbits { get; set; } = new List<Rabbit>();
        public virtual ICollection<Breeding> Breedings { get; set; } = new List<Breeding>();
        public virtual ICollection<Cage> Cages { get; set; } = new List<Cage>();


    }

    public static class FarmExtension
    {
        public static void ThrowIfNull(this Farm me)
        {
            if (me == null)
                throw new ApplicationException("Farm record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Farm me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Farm record already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class FarmConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Farm>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Farm> b)
        {
            b.ToTable("Farm");
            b.HasKey(e => e.FarmId);

            b.Property(e => e.FarmId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.Rabbits)
                .WithOne(d => d.Farm)
                .HasForeignKey(d => d.FarmId);

            b.HasMany(e => e.Breedings)
                .WithOne(d => d.Farm)
                .HasForeignKey(d => d.FarmId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasMany(e => e.Cages)
                .WithOne(d => d.Farm)
                .HasForeignKey(d => d.FarmId);
            //b.HasQueryFilter(e => e.DateEnd > DateTime.UtcNow);
        }
    }
}
