//Zuständig um alle Datenbank migrationen beim Start der Web-App anzuwenden
using KCA_AuthentificationAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace AspNetCore.Identity.Extensions;

public static class MigrationsExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();
        using AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.Migrate();
    }
}
