using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Shared.Storage.FileSystem;

public static class Extensions
{
    public static IServiceCollection AddFileSystemStorage(this IServiceCollection services, string rootPath = "")
    {
        services.Configure<FileSystemOptions>(options => options.RootPath = rootPath);
        services.TryAddEnumerable(ServiceDescriptor.Transient<IStorageProvider, FileSystemStorageProvider>());

        return services;
    }
}