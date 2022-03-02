
using Data.App.Models.Rabbits.Activities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Data.App.Models.Rabbits.Supplies
{
    public class Medicine
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string MedicineId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        public virtual ICollection<Activity> Medications { get; set; } = new List<Activity>();
    }

    public class MedicineConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Medicine>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Medicine> b)
        {
            b.ToTable("Medicine");
            b.HasKey(e => e.MedicineId);

            b.Property(e => e.MedicineId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
            b.HasIndex(e => e.Name).IsUnique();
            b.Property(e => e.Description).HasMaxLength(DescMaxLength).IsRequired();
            b.Property(e => e.Notes).HasMaxLength(NoteMaxLength).IsRequired();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            b.HasMany(e => e.Medications)
                .WithOne(d => d.Medicine)
                .HasForeignKey(d => d.MedicineId);
        }
    }
}
