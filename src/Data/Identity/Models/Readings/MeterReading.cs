using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Readings
{
    public class MeterReading
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MeterReadingId { get; set; }

        public string AccountId { get; set; }
        public virtual Account Account { get; set; }

        public double Value { get; set; }

        DateTime _dateRead;
        public DateTime DateRead
        {
            get => _dateRead.AsUtc();
            set => _dateRead = value.Truncate();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
    }

    internal class MeterReadingConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<MeterReading>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<MeterReading> b)
        {
            b.ToTable("MeterReading");
            b.HasKey(e => e.MeterReadingId);

            b.Property(e => e.MeterReadingId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.AccountId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.DateRead).IsRequired();


            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();
        }
    }
}
