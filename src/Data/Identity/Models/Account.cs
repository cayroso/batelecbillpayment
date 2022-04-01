using Data.Identity.Models.Billings;
using Data.Identity.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.Models
{
    public class Account
    {
        public string AccountId { get; set; }

        public string AccountNumber { get; set; }
        public string MeterNumber { get; set; }
        public string ConsumerType { get; set; }

        //'public string UserInformationId { get; set; }
        public virtual UserInformation UserInformation { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Billing> Billings { get; set; }

        public string ConcurrencyToken { get; set; } = Guid.NewGuid().ToString();
        //  userid/userinformationid
        //  consumer name x
        //  consumer address
        //  account no
        //  consumer type
        //  meter no
    }

    internal class AccountConfiguration : Cayent.Core.Data.Components.EntityBaseConfiguration<Account>
    {
        public override void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Account> b)
        {
            b.ToTable("Account");
            b.HasKey(e => e.AccountId);

            b.Property(e => e.AccountId).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.AccountNumber).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.MeterNumber).HasMaxLength(KeyMaxLength).IsRequired();
            b.Property(e => e.Address).HasMaxLength(KeyMaxLength).IsRequired(false);

            b.HasOne(e => e.UserInformation)
                .WithOne()
                .HasForeignKey<Account>(d => d.AccountId);

            b.HasMany(e => e.Billings)
                .WithOne(e => e.Account)
                .IsRequired();

            b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired().IsConcurrencyToken();
        }
    }
}
