using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using IdentityServer.Data.SeedData;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

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

    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var context = scope.ServiceProvider.GetService<ConfigurationDbContext>();

        if (context is null)
        {
            return;
        }

        EnsureSeedConfigurations(context);
        EnsureIdentity(scope);
    }

    private static void EnsureSeedConfigurations(ConfigurationDbContext context)
    {
        if (!context.Clients.Any())
        {
            Log.Information("Clients being populated");

            context.Clients.AddRange(Config.Clients.Select(x => x.ToEntity()));
            context.SaveChanges();
        }

        if (!context.IdentityResources.Any())
        {
            Log.Information("IdentityResources being populated");

            context.IdentityResources.AddRange(Config.IdentityResources.Select(x => x.ToEntity()));
            context.SaveChanges();
        }

        if (!context.ApiScopes.Any())
        {
            Log.Information("ApiScopes being populated");

            context.ApiScopes.AddRange(Config.ApiScopes.Select(x => x.ToEntity()));
            context.SaveChanges();
        }

        if (!context.ApiResources.Any())
        {
            Log.Information("ApiResources being populated");

            context.ApiResources.AddRange(Config.ApiResources.Select(x => x.ToEntity()));
            context.SaveChanges();
        }
    }

    private static void EnsureIdentity(IServiceScope scope)
    {
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();

        Log.Information("Roles and its claims being populated");
        IdentitySeeding.SpreadRoles(roleManager).Wait();

        Log.Information("Users and its claims being populated");
        IdentitySeeding.SpreadUsers(userManager).Wait();
    }
}