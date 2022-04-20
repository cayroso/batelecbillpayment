using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Notifications
{
    public enum EnumNotificationType
    {
        Unknown = 0,
        Primary,
        Secondary,
        Success,
        Info,
        Warning,
        Error
    }

    public enum EnumNotificationEntityClass
    {
        Unknown = 0,
        Notification = 1,
        Reservation = 2,
        Billing = 3,
        Announcement = 4,
    }

    public class Notification
    {
        public string NotificationId { get; set; }
        public EnumNotificationType NotificationType { get; set; }
        public EnumNotificationEntityClass NotificationEntityClass { get; set; }
        public string IconClass { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string ReferenceId { get; set; }

        DateTime _dateSent;
        public DateTime DateSent
        {
            get => _dateSent;
            set => _dateSent = value.Truncate().AsUtc();
        }

        public virtual ICollection<NotificationReceiver> Receivers { get; set; } = new List<NotificationReceiver>();
    }

    internal class NotificationConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Notification>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Notification> b)
        {
            b.ToTable("Notification");
            b.HasKey(e => e.NotificationId);

            b.Property(e => e.NotificationId).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.AccountId).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.BillingNumber).HasMaxLength(KeyMaxLength).IsRequired();
            //b.Property(e => e.GcashResourceId).HasMaxLength(KeyMaxLength).IsRequired(false);
            //b.Property(e => e.GcashPaymentId).HasMaxLength(KeyMaxLength).IsRequired(false);

            b.HasMany(e => e.Receivers)
                .WithOne(d => d.Notification)
                .HasForeignKey(d => d.NotificationId);
        }
    }
}
