using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PartnerWebApp.Models;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", false, true)
    .AddEnvironmentVariables()
    .Build();


// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.Configure<AppSettings>(config);
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection(AppSettings.AppConfiguration));

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "PartnerWebApp-Backend", Version = "v1" });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
}

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html"); ;

app.Run();

