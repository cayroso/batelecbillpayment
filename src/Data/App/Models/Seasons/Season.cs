using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cayent.Core.Common.Extensions;
using Data.App.Models.Breedings;
using Microsoft.EntityFrameworkCore;

namespace Data.App.Models.Seasons
{
    public class Season
    {
        public string SeasonId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        DateTime _dateStart;
        public DateTime DateStart
        {
            get => _dateStart.AsUtc();
            set => _dateStart = value.Truncate();
        }

        DateTime _dateEnd;
        public DateTime DateEnd
        {
            get => _dateEnd.AsUtc();
            set => _dateEnd = value.Truncate();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<Breeding> Breedings { get; set; } = new List<Breeding>();
        //public virtual ICollection<Batch> Batches { get; set; } = new List<Batch>();
    }

    public static class SeasonExtension
    {
        public static void ThrowIfNull(this Season me)
        {
            if (me == null)
                throw new ApplicationException("Season record not found.");
        }

        public static void ThrowIfNullOrAlreadyUpdated(this Season me, string currentToken, string newToken)
        {
            me.ThrowIfNull();

            if (string.IsNullOrWhiteSpace(newToken))
                throw new ApplicationException("Anti-forgery token not found.");

            if (me.ConcurrencyToken != currentToken)
                throw new ApplicationException("Season record already updated by another user.");

            me.ConcurrencyToken = newToken;
        }
    }

    public class SeasonConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Season>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Season> b)
        {
            b.ToTable("Season");
            b.HasKey(e => e.SeasonId);

            b.Property(e => e.SeasonId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();

            b.HasMany(e => e.Breedings)
                .WithOne(d => d.Season)
                .HasForeignKey(d => d.SeasonId);
        }
    }
}
