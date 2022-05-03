using WebRazor.Factory;
using Data.Identity.DbContext;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

[assembly: HostingStartup(typeof(Blazor.Server.Identity.IdentityHostingStartup))]
namespace Blazor.Server.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        const int SESSIONEXPIRATIONINMINUTES = 30;

        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                var config = context.Configuration;

                var useSQLite = config.GetValue<bool>("AppSettings:UseSQLite");
                var connString = useSQLite ? config.GetConnectionString("AppDbContextConnectionSQLite") : config.GetConnectionString("AppDbContextConnectionSQLServer");

                services.AddDbContext<IdentityWebContext>(options =>
                {
                    if (useSQLite)
                        options.UseSqlite(connString);
                    else
                        options.UseSqlServer(connString);
                });

                services.AddScoped<MyUserClaimsPrincipalFactory>();

                services.AddDefaultIdentity<IdentityWebUser>()
                    .AddRoles<IdentityRole>()
                    .AddEntityFrameworkStores<IdentityWebContext>()
                    .AddDefaultTokenProviders()
                    .AddClaimsPrincipalFactory<MyUserClaimsPrincipalFactory>()
                    ;


                services.Configure<IdentityOptions>(options =>
                {
                    //options.SignIn.RequireConfirmedEmail = true;	

                    // Password settings	
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequiredUniqueChars = 0;

                    // Lockout settings	
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(SESSIONEXPIRATIONINMINUTES);
                    options.Lockout.MaxFailedAccessAttempts = 5;
                    //options.Lockout.AllowedForNewUsers = false;

                    // User settings	
                    options.User.RequireUniqueEmail = true;
                });

                services.ConfigureApplicationCookie(options =>
                {
                    options.Cookie.Name = IdentityConstants.ApplicationScheme;
                    // Cookie settings	
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(SESSIONEXPIRATIONINMINUTES);
                    options.SlidingExpiration = true;
                    //options.Cookie.Expiration = TimeSpan.FromMinutes(SESSIONEXPIRATIONINMINUTES);
                    // If the LoginPath isn't set, ASP.NET Core defaults 	
                    // the path to /Account/Login.	
                    options.LoginPath = "/Account/Login";
                    //options.LogoutPath = "/Identity/Account/Logout";
                    // If the AccessDeniedPath isn't set, ASP.NET Core defaults 	
                    // the path to /Account/AccessDenied.	
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;


                    //options.Events.OnRedirectToLogin = ReplaceRedirector(StatusCodes.Status401Unauthorized, options.Events.OnRedirectToLogin);
                    //options.Events.OnRedirectToAccessDenied = ReplaceRedirector(StatusCodes.Status403Forbidden, options.Events.OnRedirectToAccessDenied);
                    options.Events.OnRedirectToLogin += new Func<Microsoft.AspNetCore.Authentication.RedirectContext<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>, Task>((a) =>
                    {
                        if (IsApiRequest(a.Request))
                            a.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        return Task.CompletedTask;
                    });
                    options.Events.OnRedirectToAccessDenied += new Func<Microsoft.AspNetCore.Authentication.RedirectContext<Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationOptions>, Task>((a) =>
                    {
                        if (IsApiRequest(a.Request))
                            a.Response.StatusCode = StatusCodes.Status401Unauthorized;// 401;

                        return Task.CompletedTask;
                    });
                });

                // Add application services.	
                services.AddTransient<IEmailSender, EmailSender>();
            });
        }

        private bool IsApiRequest(HttpRequest request)
        {
            string apiPath = "/api/";//  VirtualPathUtility.ToAbsolute("~/api/");
            return request.Path.Value.StartsWith(apiPath);// request.Uri.LocalPath.StartsWith(apiPath);
        }

        static Func<RedirectContext<CookieAuthenticationOptions>, Task> ReplaceRedirector(int statusCode, Func<RedirectContext<CookieAuthenticationOptions>, Task> existingRedirector) => context =>
        {
            //if (context.Request.Path.StartsWithSegments("/api"))
            {
                context.Response.StatusCode = statusCode;
                return Task.CompletedTask;
            }
            //return existingRedirector(context);
        };
    }

    public sealed class EmailSender : IEmailSender
    {
        Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            return Task.CompletedTask;
        }
    }
}