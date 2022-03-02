
using Data.App.Models.Rabbits.Supplies;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data.App.Models.Rabbits.Activities
{
    public class Vaccination
    {
        public string ActivityId { get; set; }
        public virtual Activity Activity { get; set; }

        public string VaccineId { get; set; }
        public virtual Vaccine Vaccine { get; set; }
    }

    public class VaccinationConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Vaccination>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Vaccination> b)
        {
            b.ToTable("Vaccination");
            b.HasKey(e => e.ActivityId);

            b.Property(e => e.ActivityId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.VaccineId).HasMaxLength(KeyMaxLength).IsRequired();
        }
    }
}
