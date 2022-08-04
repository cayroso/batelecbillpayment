using Cayent.Core.Data.Identity.Models.Users;
using Data.Constants;
using Data.Identity.Models;
using Data.Identity.Models.Billings;
using Data.Identity.Models.Reservations;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Identity.DbContext
{
    public static class IdentityWebContextInitializer
    {
        public static void Initialize(IdentityWebContext ctx)
        {
            if (ctx.Users.Any())
                return;

            CreateRoles(ctx);

            CreateAdministrator(ctx);

            CreateBrances(ctx);

            ctx.SaveChanges();
        }

        static void CreateRoles(IdentityWebContext ctx)
        {
            var roles = ApplicationRoles.Items.Select(e => new IdentityRole
            {
                Id = e.Id,
                Name = e.Name,
                NormalizedName = e.Name.ToUpper()
            });

            ctx.AddRange(roles);
        }

        static void CreateAdministrator(IdentityWebContext ctx)
        {
            var tenant = new Tenant
            {
                TenantId = "administrator",
                Name = "Administrator",
                Host = "Administrators Host",
                DatabaseConnectionString = @"Data Source=App_Data\TenantDB-DV.db;",
                PhoneNumber = "+639198262335",
                Email = "caydev2010@gmail.com",
                Address = "",

            };

            var token1 = Guid.NewGuid().ToString();

            var email1 = "system1@bbp.com";
            var user1 = new IdentityWebUser
            {
                Id = email1,
                UserName = email1,
                NormalizedUserName = email1.ToUpper(),

                Email = email1,
                NormalizedEmail = email1.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639198262335",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token1,
                UserInformation = new UserInformation
                {
                    FirstName = "Kerina",
                    LastName = "Talandipa",
                    ConcurrencyToken = token1,
                    Theme = "https://bootswatch.com/4/darkly/bootstrap.min.css"
                }
            };

            var userRole1 = new IdentityUserRole<string>
            {
                UserId = user1.Id,
                RoleId = ApplicationRoles.System.Id
            };

            var email2 = "admin1@bbp.com";
            var user2 = new IdentityWebUser
            {
                Id = email2,
                UserName = email2,
                NormalizedUserName = email2.ToUpper(),

                Email = email2,
                NormalizedEmail = email2.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639198262335",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token1,
                UserInformation = new UserInformation
                {
                    FirstName = "Tina",
                    LastName = "Moran",
                    ConcurrencyToken = token1,
                    Theme = "https://bootswatch.com/4/spacelab/bootstrap.min.css"
                }
            };
            var userRole2 = new IdentityUserRole<string>
            {
                UserId = user2.Id,
                RoleId = ApplicationRoles.Administrator.Id
            };

            var email3 = "admin2@bbp.com";
            var user3 = new IdentityWebUser
            {
                Id = email3,
                UserName = email3,
                NormalizedUserName = email3.ToUpper(),

                Email = email3,
                NormalizedEmail = email3.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639198262335",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token1,
                UserInformation = new UserInformation
                {
                    FirstName = "Chino",
                    LastName = "Pacia",
                    ConcurrencyToken = token1,
                    Theme = "https://bootswatch.com/4/spacelab/bootstrap.min.css"
                }
            };
            var userRole3 = new IdentityUserRole<string>
            {
                UserId = user3.Id,
                RoleId = ApplicationRoles.Administrator.Id
            };

            ctx.AddRange(tenant, user1, userRole1, user2, userRole2, user3, userRole3);

            //  sample consumer
            var consumerEmail = "consumer1@bbp.com";
            var consumer1 = new IdentityWebUser
            {
                Id = consumerEmail,
                UserName = consumerEmail,
                NormalizedUserName = consumerEmail.ToUpper(),

                Email = consumerEmail,
                NormalizedEmail = consumerEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639198262335",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token1,
                UserInformation = new UserInformation
                {
                    FirstName = "Chino",
                    LastName = "Pacia",
                    ConcurrencyToken = token1,
                    Theme = "https://bootswatch.com/4/spacelab/bootstrap.min.css"
                }
            };
            var consumerRole1 = new IdentityUserRole<string>
            {
                UserId = consumer1.Id,
                RoleId = ApplicationRoles.Consumer.Id
            };

            var account1 = new Account
            {
                AccountId = consumer1.Id,
                AccountNumber = "Account 001",
                Address = "123 Main Street",
                ConsumerType = "ConsumerTYpe",
                MeterNumber = "202100090084",
            };

            var now = DateTime.UtcNow;
            var dateStart = DateTime.UtcNow.AddDays(now.Day);
            var dateEnd = dateStart.AddMonths(1);

            var account1Billings = new[] {
                new Billing
                {
                    BillingId = Guid.NewGuid().ToString(),
                    AccountId = account1.AccountId,
                    Amount = 1000,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    DateDue = dateEnd.AddDays(5),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString(),
                    Number = Guid.NewGuid().ToString(),
                    KilloWattHourUsed = 102, Multiplier = 1,
                    PresentReading = 100, PreviousReading = 50,
                    Reader = "Reader#001", ReadingDate = now,
                },
                new Billing
                {
                    BillingId = Guid.NewGuid().ToString(),
                    AccountId = account1.AccountId,
                    Amount = 2000,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    DateDue = dateEnd.AddDays(5),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString(),
                    Number = Guid.NewGuid().ToString(),
                    KilloWattHourUsed = 102, Multiplier = 1,
                    PresentReading = 100, PreviousReading = 50,
                    Reader = "Reader#001", ReadingDate = now,
                },
                new Billing
                {
                    BillingId = Guid.NewGuid().ToString(),
                    AccountId = account1.AccountId,
                    Amount = 500,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    DateDue = dateEnd.AddDays(5),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString(),
                    Number = Guid.NewGuid().ToString(),
                    KilloWattHourUsed = 102, Multiplier = 1,
                    PresentReading = 100, PreviousReading = 50,
                    Reader = "Reader#002", ReadingDate = now,
                }

            };

            // sample reservations
            var reservations = new[]
            {
                new Reservation
                {
                    ReservationId = Guid.NewGuid().ToString(),
                    AccountId = consumer1.Id, 
                    BranchId = "ermita-balayan", 
                    DateReservation = now.AddDays(4)
                },
                new Reservation
                {
                    ReservationId = Guid.NewGuid().ToString(),
                    AccountId = consumer1.Id,
                    BranchId = "ermita-balayan",
                    DateReservation = now.AddDays(6)
                },
                new Reservation
                {
                    ReservationId = Guid.NewGuid().ToString(),
                    AccountId = consumer1.Id,
                    BranchId = "calaca",
                    DateReservation = now.AddDays(7)
                }
            };

            ctx.AddRange(consumer1, consumerRole1, account1);

            ctx.AddRange(account1Billings);
            ctx.AddRange(reservations);

            CreateConsumer1(ctx, tenant, token1);
            CreateConsumer2(ctx, tenant, token1);
            CreateConsumer3(ctx, tenant, token1);
        }

        static void CreateConsumer1(IdentityWebContext ctx, Tenant tenant, string token)
        {
            //  sample consumer
            var consumerEmail = "selwyn@bbp.com";
            var consumer1 = new IdentityWebUser
            {
                Id = consumerEmail,
                UserName = consumerEmail,
                NormalizedUserName = consumerEmail.ToUpper(),

                Email = consumerEmail,
                NormalizedEmail = consumerEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639076126930",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token,
                UserInformation = new UserInformation
                {
                    FirstName = "Selwyn",
                    LastName = "Santos",
                    ConcurrencyToken = token,
                    Theme = "https://bootswatch.com/4/spacelab/bootstrap.min.css"
                }
            };
            var consumerRole1 = new IdentityUserRole<string>
            {
                UserId = consumer1.Id,
                RoleId = ApplicationRoles.Consumer.Id
            };

            var account1 = new Account
            {
                AccountId = consumer1.Id,
                AccountNumber = "Account 001",
                Address = "123 Main Street",
                ConsumerType = "ConsumerType",
                MeterNumber = "202100090084",
            };

            var now = DateTime.UtcNow;
            var dateStart = DateTime.UtcNow.AddDays(now.Day);
            var dateEnd = dateStart.AddMonths(1);

            var account1Billings = new[] {
                new Billing
                {
                    BillingId = Guid.NewGuid().ToString(),
                    AccountId = account1.AccountId,
                    Amount = 1000,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    DateDue = dateEnd.AddDays(5),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString(),
                    Number = Guid.NewGuid().ToString(),
                    KilloWattHourUsed = 102, Multiplier = 1,
                    PresentReading = 300, PreviousReading = 150,
                    Reader = "Reader#001", ReadingDate = now,
                },
            };

            ctx.AddRange(consumer1, consumerRole1, account1);
            ctx.AddRange(account1Billings);
        }

        static void CreateConsumer2(IdentityWebContext ctx, Tenant tenant, string token)
        {
            //  sample consumer
            var consumerEmail = "elizabeth@bbp.com";
            var consumer1 = new IdentityWebUser
            {
                Id = consumerEmail,
                UserName = consumerEmail,
                NormalizedUserName = consumerEmail.ToUpper(),

                Email = consumerEmail,
                NormalizedEmail = consumerEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639656905272",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token,
                UserInformation = new UserInformation
                {
                    FirstName = "Elizabeth",
                    LastName = "Dela Rosa",
                    ConcurrencyToken = token,
                    Theme = "https://bootswatch.com/4/spacelab/bootstrap.min.css"
                }
            };
            var consumerRole1 = new IdentityUserRole<string>
            {
                UserId = consumer1.Id,
                RoleId = ApplicationRoles.Consumer.Id
            };

            var account1 = new Account
            {
                AccountId = consumer1.Id,
                AccountNumber = "Account 002",
                Address = "123 Main Street",
                ConsumerType = "ConsumerType",
                MeterNumber = "202100090084",
            };

            var now = DateTime.UtcNow;
            var dateStart = DateTime.UtcNow.AddDays(now.Day);
            var dateEnd = dateStart.AddMonths(1);

            var account1Billings = new[] {
                new Billing
                {
                    BillingId = Guid.NewGuid().ToString(),
                    AccountId = account1.AccountId,
                    Amount = 1000,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    DateDue = dateEnd.AddDays(5),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString(),
                    Number = Guid.NewGuid().ToString(),
                    KilloWattHourUsed = 102, Multiplier = 1,
                    PresentReading = 200, PreviousReading = 70,
                    Reader = "Reader#021", ReadingDate = now,
                },
            };

            ctx.AddRange(consumer1, consumerRole1, account1);
            ctx.AddRange(account1Billings);
        }

        static void CreateConsumer3(IdentityWebContext ctx, Tenant tenant, string token)
        {
            //  sample consumer
            var consumerEmail = "jericho@bbp.com";
            var consumer1 = new IdentityWebUser
            {
                Id = consumerEmail,
                UserName = consumerEmail,
                NormalizedUserName = consumerEmail.ToUpper(),

                Email = consumerEmail,
                NormalizedEmail = consumerEmail.ToUpper(),
                EmailConfirmed = true,
                PhoneNumber = "+639502390692",
                PhoneNumberConfirmed = true,

                LockoutEnabled = false,
                LockoutEnd = null,
                PasswordHash = "AQAAAAEAACcQAAAAEKGIieH17t5bYXa5tUfxRwN9UIEwApTKbQBRaUtIHplIUG2OfYxvBS8uvKy5E2Stsg==",
                SecurityStamp = "6SADCY3NMMLOHA2S26ZJCEWGHWSQUYRM",
                TwoFactorEnabled = false,
                AccessFailedCount = 0,
                TenantId = tenant.TenantId,
                ConcurrencyStamp = token,
                UserInformation = new UserInformation
                {
                    FirstName = "Jericho",
                    MiddleName = "Austria",
                    LastName = "Delos Santos",
                    ConcurrencyToken = token,
                    Theme = "https://bootswatch.com/4/spacelab/bootstrap.min.css"
                }
            };
            var consumerRole1 = new IdentityUserRole<string>
            {
                UserId = consumer1.Id,
                RoleId = ApplicationRoles.Consumer.Id
            };

            var account1 = new Account
            {
                AccountId = consumer1.Id,
                AccountNumber = "Account 003",
                Address = "123 Main Street",
                ConsumerType = "ConsumerType",
                MeterNumber = "202100090084",
            };

            var now = DateTime.UtcNow;
            var dateStart = DateTime.UtcNow.AddDays(now.Day);
            var dateEnd = dateStart.AddMonths(1);

            var account1Billings = new[] {
                new Billing
                {
                    BillingId = Guid.NewGuid().ToString(),
                    AccountId = account1.AccountId,
                    Amount = 1000,
                    DateStart = dateStart,
                    DateEnd = dateEnd,
                    DateDue = dateEnd.AddDays(5),
                    Month = now.Month.ToString(),
                    Year = now.Year.ToString(),
                    Number = Guid.NewGuid().ToString(),
                    KilloWattHourUsed = 102, Multiplier = 1,
                    PresentReading = 100, PreviousReading = 90,
                    Reader = "Reader#1241", ReadingDate = now,
                },
            };

            ctx.AddRange(consumer1, consumerRole1, account1);
            ctx.AddRange(account1Billings);
        }
        static void CreateBrances(IdentityWebContext ctx)
        {
            var branches = new[]
            {
                new Branch{ BranchId = "butong-taal", Name = "Butong, Taal" },
                new Branch{ BranchId = "mataas-na-bayan-lemery", Name = "Mataas na Bayan, Lemery" },
                new Branch{ BranchId = "palanas-lemery", Name = "Palanas, Lemery" },
                new Branch{ BranchId = "gulod-calatagan", Name = "Gulod, Calatagan" },
                new Branch{ BranchId = "ermita-balayan", Name = "Ermita, Balayan" },
                new Branch{ BranchId = "camp-avejar-nasugbu", Name = "Camp Avejar, Nasugbu" },
                new Branch{ BranchId = "natipuan", Name = "Natipuan" },
                new Branch{ BranchId = "calaca", Name = "Calaca" },
            };

            ctx.AddRange(branches);
        }
    }
}
