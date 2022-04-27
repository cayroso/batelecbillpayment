using Data.Identity.Models.Billings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Gcash
{
    public class GcashPayment
    {
        public string GcashPaymentId { get; set; }

        public string BillingId { get; set; }
        public virtual Billing Billing { get; set; }

        //public string GcashResourceId { get; set; }
        //public virtual GcashResource GcashResource { get; set; }

        public string AccessUrl { get; set; }
        public double Amount { get; set; }
        public string BalanceTransactionId { get; set; }
        public string Currency { get; set; }
        public string Description { get; set; }
        public bool Disputed { get; set; }
        public string ExternalReferenceNumber { get; set; }
        public double Fee { get; set; }
        public bool LiveMode { get; set; }
        public double NetAmount { get; set; }
        //public string Payout { get; set; }
        public string StatementDescriptor { get; set; }
        public string Status { get; set; }
        public double TaxAmount { get; set; }

        public DateTime Available_At { get; set; }
        public DateTime Created_At { get; set; }
        public DateTime Paid_At { get; set; }
        public DateTime Updated_At { get; set; }
    }

    internal class GcashPaymentConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<GcashPayment>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<GcashPayment> b)
        {
            b.ToTable("GcashPayment");
            b.HasKey(e => e.GcashPaymentId);

            b.Property(e => e.GcashPaymentId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BillingId).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.GcashResourceId).HasMaxLength(KeyMaxLength).IsRequired();

            b.Property(e => e.AccessUrl).HasMaxLength(DescMaxLength).IsRequired(false);
            b.Property(e => e.BalanceTransactionId).HasMaxLength(DescMaxLength).IsRequired(false);
            b.Property(e => e.Description).HasMaxLength(DescMaxLength).IsRequired(false);
            b.Property(e => e.ExternalReferenceNumber).HasMaxLength(DescMaxLength).IsRequired(false);
            //b.Property(e => e.Payout).HasMaxLength(DescMaxLength).IsRequired(false);
            b.Property(e => e.StatementDescriptor).HasMaxLength(DescMaxLength).IsRequired(false);
            b.Property(e => e.Status).HasMaxLength(DescMaxLength).IsRequired(false);
            //b.Property(e => e.Access_Url).HasMaxLength(DescMaxLength).IsRequired(false);
            //b.Property(e => e.Access_Url).HasMaxLength(DescMaxLength).IsRequired(false);

            b.HasOne(e => e.Billing);
                

            //b.HasOne(e => e.GcashResource);
        }
    }
}
