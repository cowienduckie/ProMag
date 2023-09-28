using EfCore.Auditing;
using EfCore.Auditing.Implementations;
using EfCore.Context;
using Microsoft.Extensions.DependencyInjection;

namespace EfCore;

public static class Extensions
{
    public static IServiceCollection AddAuditLogs<TContext>(this IServiceCollection services) where TContext : BaseDbContext
    {
        services.AddTransient<IAuditService, AuditService<TContext>>();

        return services;
    }
}