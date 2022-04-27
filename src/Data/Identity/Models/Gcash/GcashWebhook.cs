using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Gcash
{
    public class GcashWebhook
    {
        public string Id { get; set; }
        public bool LiveMode { get; set; }
        public string Secret_Key { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string Events { get; set; }
    }

    public class GcashWebHookListWrapper
    {
        public IEnumerable<GcashWebhook> Data { get; set; }
    }

    internal class GcashWebhookConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<GcashWebhook>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<GcashWebhook> b)
        {
            b.ToTable("GcashWebhook");
            b.HasKey(e => e.Id);

            b.Property(e => e.Id).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.BillingId).HasMaxLength(KeyMaxLength).IsRequired();

            //b.Property(e => e.CheckoutUrl).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.Status).HasMaxLength(KeyMaxLength).IsRequired();

            //b.Property(e => e.BillingName).HasMaxLength(KeyMaxLength).IsRequired(false);
            //b.Property(e => e.BillingEmail).HasMaxLength(KeyMaxLength).IsRequired(false);
            //b.Property(e => e.BillingPhone).HasMaxLength(KeyMaxLength).IsRequired(false);

            //b.HasOne(e => e.Billing);

            //b.HasOne(e => e.GcashPayment)
            //    .WithOne(d => d.GcashResource)
            //    .HasForeignKey<GcashPayment>(d => d.GcashPaymentId);
        }
    }
}
