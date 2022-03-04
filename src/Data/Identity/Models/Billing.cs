using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models
{
    public class Billing
    {
        public string BillingId { get; set; }

        public string AccountId { get; set; }
        public virtual Account Account { get; set; }

        public string BillingNumber { get; set; }
        public string BillingMonth { get; set; }
        public string BillingYear { get; set; }


        DateTime _readingDate;
        public DateTime ReadingTime
        {
            get => _readingDate.AsUtc();
            set => _readingDate = value.Truncate();
        }

        DateTime _billDateStart;
        public DateTime BillDateStart
        {
            get => _billDateStart.AsUtc();
            set => _billDateStart = value.Truncate();
        }

        DateTime _billDateEnd;
        public DateTime BillDateEnd
        {
            get => _billDateEnd.AsUtc();
            set => _billDateEnd = value.Truncate();
        }

        public double PresentReading { get; set; }
        public double PreviousReading { get; set; }
        public double Multiplier { get; set; }
        public double KilloWattHourUsed { get; set; }

        DateTime _billingDateDue;
        public DateTime BillingDateDue
        {
            get => _billingDateDue.AsUtc();
            set => _billingDateDue = value.Truncate();
        }

        public string Reader { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
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
    }

    internal class BillConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Billing>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Billing> b)
        {
            b.ToTable("Billing");
            b.HasKey(e => e.BillingId);

            b.Property(e => e.BillingId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.AccountId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BillingNumber).HasMaxLength(KeyMaxLength).IsRequired();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();
        }
    }
}
