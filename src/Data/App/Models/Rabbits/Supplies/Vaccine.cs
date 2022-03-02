
using Data.App.Models.Rabbits.Activities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.App.Models.Rabbits.Supplies
{
    public class Vaccine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string VaccineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<Activity> Vaccinations { get; set; } = new List<Activity>();
    }

    public class VaccineConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Vaccine>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Vaccine> b)
        {
            b.ToTable("Vaccine");
            b.HasKey(e => e.VaccineId);

            b.Property(e => e.VaccineId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();
            b.Property(e => e.Description).HasMaxLength(DescMaxLength).IsRequired();
            b.Property(e => e.Notes).HasMaxLength(NoteMaxLength).IsRequired();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.Vaccinations)
                .WithOne(d => d.Vaccine)
                .HasForeignKey(d => d.VaccineId);
        }
    }
}
