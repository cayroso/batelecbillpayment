
using Data.Identity.Models.Users;
using System;
using System.Collections.Generic;
using System.Text;
using Cayent.Core.Common.Extensions;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using Data.App.Models.Rabbits.Supplies;
using Data.App.Models.Users;

namespace Data.App.Models.Rabbits.Activities
{
    public class Activity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string ActivityId { get; set; }

        public string BirdId { get; set; }
        public virtual Rabbit Bird { get; set; }

        public string UserId { get; set; }
        public virtual User User { get; set; }

        public EnumActivityType ActivityType { get; set; }
        public string Description { get; set; }

        public string Notes { get; set; }

        DateTime _date;
        public DateTime Date
        {
            get => _date.AsUtc();
            set => _date = value.Truncate();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        //public virtual Feeding Feeding { get; set; }
        //public virtual Medication Medication { get; set; }
        //public virtual Vaccination Vaccination { get; set; }


        public string FeedId { get; set; }
        public virtual Feed Feed { get; set; }

        public string MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }

        public string VaccineId { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }

    public class ActivityConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Activity>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Activity> b)
        {
            b.ToTable("Activity2");
            b.HasKey(e => e.ActivityId);

            b.Property(e => e.ActivityId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BirdId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Description).HasMaxLength(DescMaxLength);
            b.Property(e => e.Notes).HasMaxLength(DescMaxLength);

            b.Property(e => e.FeedId).HasMaxLength(KeyMaxLength);

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasOne(e => e.Feed)
                .WithOne()
                .HasForeignKey<Activity>(fk => fk.FeedId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(e => e.Medicine)
                .WithOne()
                .HasForeignKey<Activity>(fk => fk.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            b.HasOne(e => e.Vaccine)
                .WithOne()
                .HasForeignKey<Activity>(fk => fk.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);


            //b.HasOne(e => e.Feeding)
            //    .WithOne(d => d.Activity)
            //    .HasForeignKey<Feeding>(d => d.ActivityId);

            //b.HasOne(e => e.Medication)
            //    .WithOne(d => d.Activity)
            //    .HasForeignKey<Medication>(d => d.ActivityId);

            //b.HasOne(e => e.Vaccination)
            //    .WithOne(d => d.Activity)
            //    .HasForeignKey<Vaccination>(d => d.ActivityId);
        }
    }
}
