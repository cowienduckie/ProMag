using MasterData.Data.SeedData;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MasterData.Data;

public static class DbInitializer
{
    public static void MigrateDatabase(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetService<MasterDataDbContext>();

        dbContext?.Database.Migrate();
    }

    public static void EnsureSeedData(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<MasterDataDbContext>();

        if (context is null)
        {
            return;
        }

        if (!context.Currencies.Any())
        {
            context.AddRange(Currencies.Seeds);
            context.SaveChanges();
        }

        if (!context.Languages.Any())
        {
            context.AddRange(Languages.Seeds);
            context.SaveChanges();
        }

        if (!context.Countries.Any())
        {
            context.AddRange(Countries.Seeds);
            context.SaveChanges();
        }

        if (!context.Timezones.Any())
        {
            context.AddRange(Timezones.Seeds);
            context.SaveChanges();
        }
    }
}