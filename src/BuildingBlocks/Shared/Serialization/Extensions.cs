using Microsoft.Extensions.DependencyInjection;

namespace Shared.Serialization;

public static class Extensions
{
    public static IServiceCollection AddCustomSerializer<TSerializer>(this IServiceCollection services) where TSerializer : class, ISerializerService
    {
        services.AddTransient<ISerializerService, TSerializer>();

        return services;
    }
}