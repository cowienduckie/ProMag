using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Shared.Storage.Internal;

namespace Shared.Storage.Extensions;

public static class StorageExtensions
{
    public static IServiceCollection AddStorage(this IServiceCollection services)
    {
        services.TryAddTransient<IStorageFactory, StorageFactory>();

        return services;
    }
}