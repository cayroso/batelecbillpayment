using Data.Identity.Models.Reservations;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models
{
    public class Branch
    {
        public string BranchId { get; set; }
        public string Name { get;set; }
        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();

        public virtual ICollection<Reservation> Reservations { get; set; }
    }

    internal class BranchConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Branch>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Branch> b)
        {
            b.ToTable("Branch");
            b.HasKey(e => e.BranchId);

            b.Property(e => e.BranchId).HasMaxLength(KeyMaxLength).IsRequired();            
            b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();

            b.HasMany(e => e.Reservations)
                .WithOne(d => d.Branch)
                .HasForeignKey(d => d.BranchId);
        }
    }
}
