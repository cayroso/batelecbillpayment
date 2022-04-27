using Cayent.Core.Common.Extensions;
using Data.Identity.Models.Gcash;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Billings
{
    public enum EnumBillingStatus
    {
        Pending = 0,
        Unpaid = 1,
        Review = 2,
        Paid = 3,
    }

    public class Billing
    {
        public string BillingId { get; set; }

        //public string GCashSourceResourceId { get; set; }
        //public string GCashCheckoutUrl { get; set; }
        public string AccountId { get; set; }
        public virtual Account Account { get; set; }

        public EnumBillingStatus Status { get; set; }

        public double Amount { get; set; }
        public string Number { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }


        DateTime _readingDate;
        public DateTime ReadingDate
        {
            get => _readingDate.AsUtc();
            set => _readingDate = value.Truncate();
        }

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

        public double PresentReading { get; set; }
        public double PreviousReading { get; set; }
        public double Multiplier { get; set; }
        public double KilloWattHourUsed { get; set; }

        DateTime _dateDue;
        public DateTime DateDue
        {
            get => _dateDue.AsUtc();
            set => _dateDue = value.Truncate();
        }

        public string Reader { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public string GcashResourceId { get; set; }
        public virtual GcashResource GcashResource { get; set; }

        public string GcashPaymentId { get; set; }
        public virtual GcashPayment GcashPayment { get; set; }

        //  bill month
        //  amount
        //  reading date
        //  bill no
        //  bill start, bill end

        //  account

        //  previous reading
        //  present reading
        //  multiplier
        //  kWH used

        //  due date
        //  reader

        public virtual ICollection<BillingAttachment> Attachments { get; set; }
    }

    internal class BillingConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Billing>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Billing> b)
        {
            b.ToTable("Billing");
            b.HasKey(e => e.BillingId);

            b.Property(e => e.BillingId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.AccountId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Number).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.GcashResourceId).HasMaxLength(KeyMaxLength).IsRequired(false);
            b.Property(e => e.GcashPaymentId).HasMaxLength(KeyMaxLength).IsRequired(false);

            b.HasOne(e => e.Account);

            b.HasOne(e => e.GcashResource)
                .WithOne(d => d.Billing)
                .HasForeignKey<GcashResource>(d => d.BillingId);

            b.HasOne(e => e.GcashPayment)
                .WithOne(d => d.Billing)
                .HasForeignKey<GcashPayment>(d => d.BillingId);

            b.HasMany(e => e.Attachments)
                .WithOne(d => d.Billing)
                .HasForeignKey(d => d.BillingId);

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();
        }
    }
}
