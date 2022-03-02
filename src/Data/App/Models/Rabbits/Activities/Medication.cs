
using Data.App.Models.Rabbits.Supplies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.App.Models.Rabbits.Activities
{
    public class Medication
    {
        public string ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public string MedicineId { get; set; }
        public virtual Medicine Medicine { get; set; }
    }

    public class MedicationConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Medication>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Medication> b)
        {
            b.ToTable("Medication");
            b.HasKey(e => e.ActivityId);

            b.Property(e => e.ActivityId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.MedicineId).HasMaxLength(KeyMaxLength).IsRequired();
        }
    }
}
