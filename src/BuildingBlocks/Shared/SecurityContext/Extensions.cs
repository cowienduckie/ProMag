using Microsoft.Extensions.DependencyInjection;
using Shared.SecurityContext.Implementations;

namespace Shared.SecurityContext;

public static class Extensions
{
    public static IServiceCollection AddSecurityContext(this IServiceCollection services)
    {
        services
            .AddHttpContextAccessor()
            .AddScoped<ISecurityContextAccessor, SecurityContextAccessor>();

        return services;
    }
}