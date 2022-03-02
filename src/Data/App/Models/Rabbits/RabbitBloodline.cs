
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.App.Models.Rabbits
{
    public class RabbitBloodline
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string RabbitBloodlineId { get; set; }

        public string RabbitId { get; set; }
        public virtual Rabbit Rabbit { get; set; }

        public string BloodlineId { get; set; }
        public virtual Bloodline Bloodline { get; set; }

        public EnumRabbitGender Gender { get; set; }
        public double Percentage { get; set; }
    }

    public class RabbitBloodlineConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<RabbitBloodline>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<RabbitBloodline> b)
        {
            b.ToTable("RabbitBloodline");
            b.HasKey(e => e.RabbitBloodlineId);
            b.HasIndex(e => new { e.RabbitId, e.BloodlineId }).IsUnique();

            b.Property(e => e.RabbitBloodlineId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.RabbitId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BloodlineId).HasMaxLength(KeyMaxLength).IsRequired();
        }
    }
}
