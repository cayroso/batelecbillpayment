using Cayent.Core.Data.Identity.Models.Users;
using Data.Constants;
using Data.Identity.Models;
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

            var email1 = "user1@rms.com";
            var token1 = Guid.NewGuid().ToString();
            var customer1 = new IdentityWebUser
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

            var customerRole1 = new IdentityUserRole<string>
            {
                UserId = customer1.Id,
                RoleId = ApplicationRoles.System.Id
            };

            var email2 = "user2@rms.com";
            var customer2 = new IdentityWebUser
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
            var customerRole2 = new IdentityUserRole<string>
            {
                UserId = customer2.Id,
                RoleId = ApplicationRoles.System.Id
            };

            ctx.AddRange(tenant, customer1, customerRole1, customer2, customerRole2);
        }

    }
}
