using AspNetCore.Identity.Extensions;
using AspNetCoreRateLimit;
using KCA_AuthentificationAPI.Data;
using KCA_AuthentificationAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// DB Context hinzufügen
builder.Services.AddDbContext<AppDbContext>(options =>
     options.UseMySql(
         builder.Configuration.GetConnectionString("DefaultConnection"),
         ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection")),
         mySqlOptions => mySqlOptions.SchemaBehavior(MySqlSchemaBehavior.Ignore)
     ));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader());
});

builder.Services.AddIdentity<AppUser, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();


// Controller und Swagger hinzufügen
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rate Limiting
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// MVC hinzufügen
builder.Services.AddMvc();
builder.WebHost.UseUrls("https://localhost:7080");
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenLocalhost(7080, listenOptions =>
    {
        listenOptions.UseHttps();
    });
});

builder.Services.AddScoped<TokenService>();
builder.Services.AddSingleton<TimeProvider>(TimeProvider.System);
builder.Services.AddSingleton<IEmailSender<AppUser>, DummyEmailSender>();


var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
    app.ApplyMigrations();
}
app.UseHttpsRedirection();


// Rate Limiting anwenden
app.UseIpRateLimiting();

// Autorisierung und Controller-Mapping
app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<AppUser>();

// Anwendung starten
app.Run();

