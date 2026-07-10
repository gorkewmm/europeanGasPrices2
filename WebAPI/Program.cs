using Business.Abstract;
using Business.Concrete;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Microsoft.EntityFrameworkCore;
using WebAPI.Service.Abstract;
using WebAPI.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

//var connectionString = builder.Configuration.GetConnectionString("EpdkByCountries");

// Add services to the container.
builder.Services.AddScoped<IFuelPriceService, FuelPriceManager>();
builder.Services.AddScoped<IFuelPriceDal, EfFuelPriceDal>();
builder.Services.AddScoped<IFuelImportService, FuelImportManager>();

builder.Services.AddHttpClient();

// InitialContext'i SQL Server kullanacak şekilde IoC Container'a kaydediyoruz
//builder.Services.AddDbContext<InitialContext>(options =>
    //options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DataAccess")));


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
