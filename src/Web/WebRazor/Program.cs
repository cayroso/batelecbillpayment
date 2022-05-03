using App.Hubs;
using Data.App.DbContext;
using Data.Constants;
using Data.Identity.DbContext;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebRazor;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => { });

builder.Services.AddSignalR();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages()
    .AddRazorPagesOptions(opt =>
    {
        opt.Conventions.AuthorizeAreaFolder("System", "/", ApplicationRoles.SystemRoleName);
        opt.Conventions.AuthorizeAreaFolder("Administrator", "/", ApplicationRoles.AdministratorRoleName);
        opt.Conventions.AuthorizeAreaFolder("Consumer", "/", ApplicationRoles.ConsumerRoleName);
    });
builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});
builder.Services.Configure<RouteOptions>(opt =>
{
    opt.LowercaseUrls = true;
    opt.LowercaseQueryStrings = true;
});

builder.Services.AddSignalR().AddNewtonsoftJsonProtocol(options =>
{
    //options.PayloadSerializerSettings.ContractResolver = new CamelCaseContractResolver();
    options.PayloadSerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    //options.PayloadSerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mmZ";
    //options.PayloadSerializerSettings.Culture = cultureInfo;
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(ApplicationRoles.SystemRoleName, policy =>
       policy.RequireAssertion(context =>
           context.User.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == ApplicationRoles.SystemRoleName)));

    options.AddPolicy(ApplicationRoles.AdministratorRoleName, policy =>
       policy.RequireAssertion(context =>
           context.User.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == ApplicationRoles.AdministratorRoleName)));

    options.AddPolicy(ApplicationRoles.ConsumerRoleName, policy =>
       policy.RequireAssertion(context =>
           context.User.HasClaim(c => c.Type == System.Security.Claims.ClaimTypes.Role && c.Value == ApplicationRoles.ConsumerRoleName)));
});

//builder.Services.AddScoped<App.Services.ChatService>();
builder.Services.AddScoped<App.Services.NotificationService>();

//builder.Services.AddTransient<ChatHub>();
//builder.Services.AddTransient<NotificationHub>();

//#if !DEBUG
builder.Services.AddProgressiveWebApp();
//#endif


StartupExtension.RegisterCQRS(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

//app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
//app.UseStaticFiles(new StaticFileOptions()
//{
//    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), @"StaticFiles")),
//    RequestPath = new PathString("/StaticFiles")
//});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapControllers();

    endpoints.MapHub<NotificationHub>("/notificationHub");
    //endpoints.MapHub<JobHub>("/jobHub");
    //endpoints.MapHub<ChatHub>("/chatHub");
});


//app.MapRazorPages();
//app.MapControllers();
//app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    var ctx1 = serviceProvider.GetRequiredService<IdentityWebContext>();
    if (ctx1.Database.GetPendingMigrations().Any())
    {
        ctx1.Database.Migrate();
    }

    IdentityWebContextInitializer.Initialize(ctx1);
}

app.Run();
