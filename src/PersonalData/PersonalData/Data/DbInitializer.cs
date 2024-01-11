using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PersonalData.Data;

public static class DbInitializer
{
    public static void MigrateDatabase(WebApplication app)
    {
        using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

        var dbContext = scope.ServiceProvider.GetService<PersonalContext>();

        dbContext?.Database.Migrate();
    }
}