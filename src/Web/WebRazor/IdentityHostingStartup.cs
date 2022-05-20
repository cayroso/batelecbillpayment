using WebRazor.Factory;
using Data.Identity.DbContext;
using Data.Identity.Models.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using SendGrid.Extensions.DependencyInjection;
using SendGrid;
using SendGrid.Helpers.Mail;

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

                services.AddSendGrid(options =>
                {                    
                    options.ApiKey = "";
                });
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
        readonly IConfiguration _configuration;
        readonly ISendGridClient _client;

        public EmailSender(IConfiguration configuration, ISendGridClient client)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }


        async Task IEmailSender.SendEmailAsync(string email, string subject, string htmlMessage)
        {
            //using mailgun
            //using var client = new HttpClient
            //{
            //    BaseAddress = new Uri("https://api.mailgun.net/v3/")
            //};
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes("api:371a94ef4df4f9c606b495aefa611214-65b08458-1847d694")));

            //var content = new FormUrlEncodedContent(new[]
            //{
            //        new KeyValuePair<string, string>("from", "postmaster@sandbox25884ca30c934aa79d4724fcecaaf539.mailgun.org"),
            //        new KeyValuePair<string, string>("to", email),
            //        new KeyValuePair<string, string>("subject", subject),
            //        new KeyValuePair<string, string>("html", htmlMessage)
            //    });

            //await client.PostAsync("sandbox25884ca30c934aa79d4724fcecaaf539.mailgun.org/messages", content).ConfigureAwait(false);

            //  using sendgrid
            var from = new EmailAddress("caydev2010@gmail.com", "Batelec Billing Payment System");
            var to = new EmailAddress(email);

            var msg = new SendGridMessage()
            {
                From = from,
                Subject = subject,
                //PlainTextContent = plainTextContent,
                HtmlContent = htmlMessage
            };
            //msg.AddContent(MimeType.Text, "and easy to do anywhere, even with C#");
            //foreach (var email in emails)
            {
                msg.AddTo(to);
            }
            //msg.MailSettings = new MailSettings
            //{
            //    SandboxMode = new SandboxMode
            //    {
            //        Enable = true
            //    },
            //};

            Console.WriteLine($"Sending email with payload: \n{msg.Serialize()}");
            var response = await _client.SendEmailAsync(msg);

            Console.WriteLine($"Response: {response.StatusCode}");
            Console.WriteLine(response.Headers);
        }
    }
}