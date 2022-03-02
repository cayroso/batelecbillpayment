using Data.App.Models.Rabbits;
using Data.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.App.Models.Breedings
{
    public class BreedingParent
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string BreedingParentId { get; set; }

        public string BreedingId { get; set; }
        public virtual Breeding Breeding { get; set; }

        public string RabbitId { get; set; }
        public virtual Rabbit Rabbit { get; set; }

        public EnumRabbitGender RabbitGender { get; set; }
    }

    public class BreedingParentConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<BreedingParent>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<BreedingParent> b)
        {
            b.ToTable("BreedingParent");
            b.HasKey(e => e.BreedingParentId);
            b.HasIndex(e => new { e.BreedingId, e.RabbitId }).IsUnique();

            b.Property(e => e.BreedingParentId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BreedingId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.RabbitId).HasMaxLength(KeyMaxLength).IsRequired();
        }
    }
}
