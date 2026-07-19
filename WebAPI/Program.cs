using Business.Abstract;
using Business.Concrete;
using Core.Utilities.Security.Encryption;
using Core.Utilities.Security.JWT;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebAPI.Service.Abstract;
using WebAPI.Service.Concrete;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("EpdkByCountries");

// Add services to the container.
builder.Services.AddScoped<IFuelPriceService, FuelPriceManager>();
builder.Services.AddScoped<IFuelPriceDal, EfFuelPriceDal>();
builder.Services.AddHttpClient<IFuelImportService, FuelImportManager>();

builder.Services.AddScoped<IJobService, JobManager>();

builder.Services.AddScoped<IUserService, UserManager>();
builder.Services.AddScoped<IUserDal, EfUserDal>();
builder.Services.AddScoped<IAuthService, AuthManager>();
builder.Services.AddScoped<ITokenHelper, JwtHelper>();

builder.Services.AddScoped<IPermissionDal, EfPermissionDal>();
builder.Services.AddScoped<IOperationClaimPermissionDal, EfOperationClaimPermissionDal>();

builder.Services.AddScoped<IPermissionService, PermissionManager>();
builder.Services.AddScoped<IOperationClaimPermissionService, OperationClaimPermissionManager>();

builder.Services.AddScoped<IOperationClaimDal, EfOperationClaimDal>();
builder.Services.AddScoped<IOperationClaimService, OperationClaimManager>();

builder.Services.AddScoped<IUserOperationClaimDal, EfUserOperationClaimDal>();
builder.Services.AddScoped<IUserOperationClaimService, UserOperationClaimManager>();

builder.Services.AddScoped<IOperationClaimPermissionDal, EfOperationClaimPermissionDal>();
builder.Services.AddScoped<IOperationClaimPermissionService, OperationClaimPermissionManager>();


//Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
    AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey),
            ClockSkew = TimeSpan.Zero // Token süresi biter bitmez erişimi kesmek için
        };
    });


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
//builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

//Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//app.UseHangfireDashboard();

//if (!app.Environment.IsDevelopment())
//{
//    using (var scope = app.Services.CreateScope())
//    {
//        var jobService = scope.ServiceProvider.GetRequiredService<IJobService>();

//        // Önce eski istemediğin jobları temizler
//        jobService.RemoveJobs();

//        // Sonra güncel jobları kaydeder/günceller
//        jobService.Jobs();
//    }
//}

app.MapControllers();

app.Run();