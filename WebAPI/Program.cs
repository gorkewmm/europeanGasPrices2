using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using WebAPI.Service.Abstract;
using WebAPI.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("EpdkByCountries");

// Add services to the container.
builder.Services.AddScoped<IFuelPriceService, FuelPriceManager>();
builder.Services.AddScoped<IFuelPriceDal, EfFuelPriceDal>();
builder.Services.AddHttpClient<IFuelImportService, FuelImportManager>();

builder.Services.AddHttpClient();

// InitialContext'i SQL Server kullanacak şekilde IoC Container'a kaydediyoruz
//builder.Services.AddDbContext<InitialContext>(options =>
    //options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DataAccess")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Hangfire bağlantı ayarları
builder.Services.AddHangfire(config =>
{
    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("EpdkByCountries"));
});
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseHangfireDashboard();

//// Benden istenilen otomatik tetikleme için Hangfire kullanıcam..
RecurringJob.AddOrUpdate<IFuelImportService>(
    "OtomatikAkaryakitVeriCekmeGörevi",
    x => x.ImportAndSaveFuelPricesAsync(),
    "57 16 * * *",
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });

//RecurringJob.AddOrUpdate<IFuelImportService>(
//    "OtomatikAkaryakitVeriCekmeGörevi2",
//    x => x.ImportAndSaveFuelPricesAsync(),
//    "14 16 * * *",
//    new RecurringJobOptions
//    {
//        TimeZone = TimeZoneInfo.Local
//    });

//RecurringJob.AddOrUpdate<IFuelImportService>(
//    "OtomatikAkaryakitVeriCekmeGörevi3",
//    x => x.ImportAndSaveFuelPricesAsync(),
//    "30 16 * * *",
//    new RecurringJobOptions
//    {
//        TimeZone = TimeZoneInfo.Local
//    });

//RecurringJob.AddOrUpdate<IFuelImportService>(
//    "OtomatikAkaryakitVeriCekmeGörevi4",
//    x => x.ImportAndSaveFuelPricesAsync(),
//    "31 16 * * *",
//    new RecurringJobOptions
//    {
//        TimeZone = TimeZoneInfo.Local
//    });


RecurringJob.AddOrUpdate<IFuelImportService>(
    "OtomatikAkaryakitVeriCekmeGörevi5",
    x => x.ImportAndSaveFuelPricesAsync(),
    "03 17 * * *",
    new RecurringJobOptions
    {
        TimeZone = TimeZoneInfo.Local
    });


app.MapControllers();

app.Run();
