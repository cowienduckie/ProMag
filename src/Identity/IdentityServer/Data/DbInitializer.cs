using Duende.IdentityServer.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Data;

public static class DbInitializer
{
    public static void MigrateDatabase(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var applicationDbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();
        var persistedGrantDbContext = scope.ServiceProvider.GetService<PersistedGrantDbContext>();
        var configurationDbContext = scope.ServiceProvider.GetService<ConfigurationDbContext>();

        applicationDbContext?.Database.Migrate();
        persistedGrantDbContext?.Database.Migrate();
        configurationDbContext?.Database.Migrate();
    }
}