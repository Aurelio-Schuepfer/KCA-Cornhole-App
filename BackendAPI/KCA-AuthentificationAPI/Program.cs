//Start der Web-App und Konfiguration der Services
using AspNetCore.Identity.Extensions;
using AspNetCoreRateLimit; //für Rate Limiting, um die Anzahl der Anfragen pro IP-Adresse zu begrenzen
using KCA_AuthentificationAPI.Data;
using KCA_AuthentificationAPI.Models;
using Microsoft.AspNetCore.Identity; //für ASP.NET Core Identity, um Benutzer zu verwalten
using Microsoft.EntityFrameworkCore; //für Entity Framework Core Unterstützung, Entity Framework Core ist ein ORM (Object-Relational Mapper) für .NET
using Pomelo.EntityFrameworkCore.MySql.Infrastructure; //für MySQL Unterstützung

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

    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    });


// E-Mail Einstellungen und E-Mail Sender hinzufügen
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton(sp =>
    sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<EmailSettings>>().Value);
builder.Services.AddSingleton<IEmailSender<AppUser>, MailKitEmailSender>();

// Controller und Swagger hinzufügen
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Rate Limiting
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

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

app.UseIpRateLimiting();

app.UseAuthorization();
app.MapControllers();
app.MapIdentityApi<AppUser>();

// Anwendung starten
app.Run();

