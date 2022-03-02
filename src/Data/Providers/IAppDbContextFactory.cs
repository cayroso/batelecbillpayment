using Cayent.Core.Data.Identity.Models;
using Data.App.DbContext;
using Data.App.Models.Calendars;
using Data.App.Models.Users;
using Data.Constants;
using Data.Identity.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Providers
{
    public interface IAppDbContextFactory
    {
        void Provision(Tenant tenant, List<ProvisionUserRole> users, bool isDevelopment);

        AppDbContext Get();

        string GenerateConnectionString(string serverName, string databaseName);
    }

    public class DefaultAppDbContextFactory : IAppDbContextFactory//,  IDesignTimeDbContextFactory<AppDbContext>	
    {
        readonly DbContextOptions<AppDbContext> _options;
        readonly IConfiguration _configuration;
        readonly ITenantProvider _tenantProvider;

        public DefaultAppDbContextFactory(DbContextOptions<AppDbContext> options, IConfiguration configuration, ITenantProvider tenantProvider)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _tenantProvider = tenantProvider ?? throw new ArgumentNullException(nameof(tenantProvider));
        }

        public void Provision(Tenant tenant, List<ProvisionUserRole> provUserRoles, bool isDevelopment)
        {
            var ctx = new AppDbContext(_options, _configuration, new DummyTenantProvider(tenant));

            if (!ctx.Database.CanConnect())
                ctx.Database.Migrate();

            var roles = new List<Role>();
            
            //  create roles
            ApplicationRoles.Items.ForEach(r =>
            {
                var role = new Role
                {
                    RoleId = r.Id,
                    Name = r.Name
                };

                roles.Add(role);
            });

            ctx.AddRange(roles);


            //  add user then assign roles
            provUserRoles.ForEach(provUserRole =>
            {
                var userRoles = new List<UserRole>();

                provUserRole.RoleIds.ForEach(roleId =>
                {
                    var userRole = new UserRole
                    {
                        UserId = provUserRole.User.UserId,
                        RoleId = roleId
                    };

                    userRoles.Add(userRole);
                });

                ctx.Add(provUserRole.User);
                ctx.AddRange(userRoles);
            });

            //  add default data
            if (!ctx.Calendars.Any() && isDevelopment)
            {
                GenerateCalendar(ctx, tenant);
                AppDbContextInitializer.Initialize(ctx, provUserRoles);
            }

            ctx.SaveChanges();
        }

        public AppDbContext Get()
        {
            var ctx = new AppDbContext(_options, _configuration, _tenantProvider);

            return ctx;
        }

        public string GenerateConnectionString(string serverName, string databaseName)
        {
            var ctx = new AppDbContext(_options, _configuration, _tenantProvider);
            var connString = string.Empty;

            if (ctx.Database.IsSqlServer())
            {
                connString = $@"Server={serverName};Database={databaseName};trusted_connection=true;";
            }
            else if (ctx.Database.IsSqlite())
            {
                connString = $"Data Source=App_Data\\{databaseName}.db;";
            }
            else
            {
                throw new ApplicationException($"Not supported database provider; {ctx.Database.ProviderName}");
            }

            return connString;
        }

        void GenerateCalendar(AppDbContext ctx, Tenant tenant)
        {
            var utcnow = DateTime.UtcNow;

            var start = utcnow.AddDays(-utcnow.DayOfYear);// new DateTime(2020, 1, 1);
            var end = start.AddYears(1);// new DateTime(2022, 12, 31);

            var now = start;

            var cals = new List<Calendar>();

            while (now <= end)
            {
                var cal = new Calendar
                {
                    Date = now,
                    Year = now.Year,
                    Month = now.Month,
                    Day = now.Day,
                    Quarter = (now.Month + 2) / 3,
                    MonthName = now.ToString("MMMMM"),
                    DayOfYear = now.DayOfYear,
                    DayOfWeek = (int)now.DayOfWeek,
                    DayName = now.ToString("dddd")
                };

                cals.Add(cal);

                now = now.AddDays(1);
            }

            ctx.AddRange(cals);
        }

    }

    public class ProvisionUserRole
    {
        public User User { get; set; }
        public List<string> RoleIds { get; set; } = new List<string>();

    }
}
