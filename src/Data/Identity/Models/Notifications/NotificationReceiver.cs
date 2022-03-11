using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Notifications
{
    public class NotificationReceiver
    {
        public string NotificationId { get; set; }
        public virtual Notification Notification { get; set; }

        public string ReceiverId { get; set; }
        public virtual Account Receiver { get; set; }


        DateTime _dateReceived;
        public DateTime DateReceived
        {
            get => _dateReceived;
            set => _dateReceived = value.Truncate().AsUtc();
        }

        DateTime _dateRead;
        public DateTime DateRead
        {
            get => _dateRead;
            set => _dateRead = value.Truncate().AsUtc();
        }

        public bool IsRead => DateTime.UtcNow > DateRead;
    }

    internal class NotificationReceiverConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<NotificationReceiver>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<NotificationReceiver> b)
        {
            b.ToTable("NotificationReceiver");
            //b.HasKey(e => e.NotificationReceiverId);
            b.HasKey(e => new { e.NotificationId, e.ReceiverId });

            b.Property(e => e.NotificationId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.ReceiverId).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.BillingNumber).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.GcashResourceId).HasMaxLength(KeyMaxLength).IsRequired(false);
            //b.Property(e => e.GcashPaymentId).HasMaxLength(KeyMaxLength).IsRequired(false);

            b.HasOne(e => e.Notification);
            b.HasOne(e => e.Receiver);

            //b.HasOne(e => e.GcashResource)
            //    .WithOne(d => d.Billing)
            //    .HasForeignKey<GcashResource>(d => d.BillingId);

            //b.HasOne(e => e.GcashPayment)
            //    .WithOne(d => d.Billing)
            //    .HasForeignKey<GcashPayment>(d => d.BillingId);


            //b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();
        }
    }
}
