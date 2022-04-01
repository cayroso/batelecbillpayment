using Data.Identity.Models.Billings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models
{
    public class GcashResource
    {
        public string GcashResourceId { get; set; }

        public string BillingId { get; set; }
        public virtual Billing Billing { get; set; }

        //public string GcashPaymentId { get; set; }
        //public virtual GcashPayment GcashPayment { get; set; }

        public double Amount { get; set; }
        public string CheckoutUrl { get; set; }
        public string Status { get; set; }
    }

    internal class GcashResourceConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<GcashResource>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<GcashResource> b)
        {
            b.ToTable("GcashResource");
            b.HasKey(e => e.GcashResourceId);

            b.Property(e => e.GcashResourceId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BillingId).HasMaxLength(KeyMaxLength).IsRequired();

            b.Property(e => e.CheckoutUrl).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Status).HasMaxLength(KeyMaxLength).IsRequired();

            //b.Property(e => e.BillingName).HasMaxLength(KeyMaxLength).IsRequired(false);
            //b.Property(e => e.BillingEmail).HasMaxLength(KeyMaxLength).IsRequired(false);
            //b.Property(e => e.BillingPhone).HasMaxLength(KeyMaxLength).IsRequired(false);

            b.HasOne(e => e.Billing);

            //b.HasOne(e => e.GcashPayment)
            //    .WithOne(d => d.GcashResource)
            //    .HasForeignKey<GcashPayment>(d => d.GcashPaymentId);
        }
    }
}
