using Cayent.Core.Common.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models.Reservations
{
    public class Reservation
    {
        public string ReservationId { get; set; }

        public string BranchId { get; set; }
        public virtual Branch Branch { get; set; }

        public string AccountId { get; set; }
        public virtual Account Account { get; set; }


        DateTime _dateReservation;
        public DateTime DateReservation
        {
            get => _dateReservation.AsUtc();
            set => _dateReservation = value.Truncate();
        }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
    }

    internal class ReservationConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Reservation>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Reservation> b)
        {
            b.ToTable("Reservation");
            b.HasKey(e => e.ReservationId);

            b.Property(e => e.ReservationId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.BranchId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.AccountId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.DateReservation).IsRequired();
            

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();
        }
    }
}
