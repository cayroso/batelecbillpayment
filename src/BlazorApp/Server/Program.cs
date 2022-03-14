using App.Hubs;
using BlazorApp.Server;
using Data.App.DbContext;
using Data.Identity.DbContext;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(opt => { });

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
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

builder.Services.AddScoped<App.Services.ChatService>();
builder.Services.AddScoped<App.Services.NotificationService>();

//builder.Services.AddTransient<ChatHub>();
//builder.Services.AddTransient<NotificationHub>();


StartupExtension.RegisterCQRS(builder.Services, builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

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
app.MapFallbackToFile("index.html");

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
