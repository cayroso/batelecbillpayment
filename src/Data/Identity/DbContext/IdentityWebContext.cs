
using Data.Identity.Models;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.DbContext
{
    public class IdentityWebContext : IdentityDbContext<IdentityWebUser, IdentityRole, string, IdentityUserClaim<string>,
        IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        #region configuration

        readonly bool _useSQLite;
        readonly string _connString;
        readonly IConfiguration _configuration;

        #endregion


        const int KeyMaxLength = 36;
        const int NameMaxLength = 256;
        const int DescMaxLength = 2048;

        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<UserInformation> UserInformations { get; set; }
        public DbSet<LoginAudit> LoginAudits { get; set; }

        //  app specific
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Billing> Billings { get; set; }
        public DbSet<GcashResource> GcashResources { get; set; }
        public DbSet<GcashPayment> GcashPayments { get; set; }


        public IdentityWebContext(DbContextOptions<IdentityWebContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;

            _useSQLite = _configuration.GetValue<bool>("AppSettings:UseSQLite");

            _connString = _useSQLite ? _configuration.GetConnectionString("AppDbContextConnectionSQLite") : _configuration.GetConnectionString("AppDbContextConnectionSQLServer");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_useSQLite)
            {
                optionsBuilder.UseSqlite(_connString);
            }
            else
            {
                optionsBuilder.UseSqlServer(_connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<Feedback>(b =>
            {
                b.ToTable("Feedback");
                b.HasKey(e => e.FeedbackId);

            });

            //builder.Entity<IdentityRole>()
            //    .HasData(ApplicationRoles.Items.Select(e => new IdentityRole
            //    {
            //        Id = e.Id,
            //        Name = e.Name,
            //        NormalizedName = e.Name.ToUpper()
            //    }));

            CreatingIdentity(builder);

            builder.Entity<Tenant>(b =>
            {
                b.ToTable("Tenant");
                b.HasKey(q => q.TenantId);

                b.Property(e => e.TenantId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
                b.Property(e => e.Host).HasMaxLength(NameMaxLength);
                b.Property(e => e.DatabaseConnectionString).HasMaxLength(DescMaxLength);
                b.Property(e => e.PhoneNumber).HasMaxLength(NameMaxLength).IsRequired();
                b.Property(e => e.Email).HasMaxLength(NameMaxLength).IsRequired();
                b.Property(e => e.Address).HasMaxLength(DescMaxLength).IsRequired();

                b.HasMany(e => e.Users)
                    .WithOne(d => d.Tenant)
                    .HasForeignKey(d => d.TenantId);
            });
        }

        void CreatingIdentity(ModelBuilder builder)
        {

            builder.Entity<IdentityWebUser>(b =>
            {
                b.ToTable("User");

                b.Property(q => q.Id).HasColumnName("UserId").HasMaxLength(KeyMaxLength);

                b.Property(e => e.UserName).HasMaxLength(NameMaxLength).IsRequired();
                b.HasIndex(e => e.UserName).IsUnique();

                b.Property(e => e.Email).HasMaxLength(NameMaxLength).IsRequired();
                b.HasIndex(e => e.Email).IsUnique();

                b.Property(e => e.TenantId).HasMaxLength(KeyMaxLength);

                b.HasOne(e => e.UserInformation)
                    .WithOne(d => d.User)
                    .HasForeignKey<UserInformation>(d => d.UserId);

                b.HasMany(e => e.LoginAudits)
                    .WithOne(d => d.User)
                    .HasForeignKey(d => d.UserId);
            });

            builder.Entity<IdentityRole>(b =>
            {
                b.ToTable("Role");

                b.Property(e => e.Id).HasColumnName("RoleId").HasMaxLength(KeyMaxLength);
                b.Property(e => e.Name).HasMaxLength(NameMaxLength).IsRequired();
                b.HasIndex(e => e.Name).IsUnique();
            });

            builder.Entity<IdentityUserRole<string>>(b =>
            {
                b.ToTable("UserRole");

                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength);
                b.Property(e => e.RoleId).HasMaxLength(KeyMaxLength);

            });

            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserToken");


            builder.Entity<UserAddress>(b =>
            {
                b.ToTable("UserAddress");
                b.HasKey(e => e.UserAddressId);

                b.Property(e => e.UserAddressId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.Address).HasMaxLength(DescMaxLength).IsRequired();
                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();

            });

            builder.Entity<UserInformation>(b =>
            {
                b.ToTable("UserInformation");
                b.HasKey(e => e.UserId);

                //b.Property(e => e.UserInformationId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.ImageId).HasMaxLength(KeyMaxLength).IsRequired(false);
                b.Property(e => e.FirstName).HasMaxLength(DescMaxLength).IsRequired();
                b.Property(e => e.MiddleName).HasMaxLength(DescMaxLength).IsRequired(false);
                b.Property(e => e.LastName).HasMaxLength(DescMaxLength).IsRequired();
                b.Property(e => e.Theme).HasMaxLength(DescMaxLength).IsRequired(false);
                b.Property(e => e.ConcurrencyToken).HasMaxLength(KeyMaxLength).IsRequired();
            });

            builder.Entity<LoginAudit>(b =>
            {
                b.ToTable("LoginAudit");
                b.HasKey(e => e.LoginAuditId);

                b.Property(e => e.LoginAuditId).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.UserId).HasMaxLength(KeyMaxLength).IsRequired();

                b.Property(e => e.RemoteIpAddress).HasMaxLength(KeyMaxLength).IsRequired();
                b.Property(e => e.LoginDate).HasMaxLength(KeyMaxLength).IsRequired();
            });

            //  app specific
            builder.ApplyConfiguration(new AccountConfiguration());
            builder.ApplyConfiguration(new BillConfiguration());
            builder.ApplyConfiguration(new GcashResourceConfiguration());
            builder.ApplyConfiguration(new GcashPaymentConfiguration());
        }
    }
}
