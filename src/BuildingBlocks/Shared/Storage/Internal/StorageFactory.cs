using Microsoft.Extensions.Options;

namespace Shared.Storage.Internal;

public class StorageFactory : IStorageFactory
{
    private readonly IOptions<StorageOptions> _options;
    private readonly IEnumerable<IStorageProvider> _storageProviders;

    public StorageFactory(IEnumerable<IStorageProvider> storageProviders, IOptions<StorageOptions> options)
    {
        _storageProviders = storageProviders;
        _options = options;
    }

    public IStore? GetStore(string storeName)
    {
        var storageOptions = _options.Value.Stores[storeName];

        return _storageProviders.FirstOrDefault(x => x.Name == storageOptions.Provider)?.BuildStore(storeName, storageOptions);
    }
}