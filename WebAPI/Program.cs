using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using WebAPI.Service.Abstract;
using WebAPI.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EpdkByCountries");

// Add services to the container.
builder.Services.AddScoped<IFuelPriceService, FuelPriceManager>();
builder.Services.AddScoped<IFuelPriceDal, EfFuelPriceDal>();
builder.Services.AddHttpClient<IFuelImportService, FuelImportManager>();

builder.Services.AddScoped<IJobService, JobManager>();

builder.Services.AddHttpClient();

builder.Services.AddDbContext<PostgreContext>();

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Hangfire bağlantı ayarları mssql
//builder.Services.AddHangfire(config =>
//{
//    config.UseSqlServerStorage(builder.Configuration.GetConnectionString("EpdkByCountries"));
//});

// Hangfire PostgreSQL bağlantı ayarları
builder.Services.AddHangfire(config =>
{
    config.UsePostgreSqlStorage(c => c.UseNpgsqlConnection(connectionString));
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

using (var scope = app.Services.CreateScope())
{
    var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

    // Önce eski istemediğin jobları temizler
    jobService.RemoveJobs();

    // Sonra güncel jobları kaydeder/günceller
    jobService.Jobs();
}

app.MapControllers();

app.Run();
