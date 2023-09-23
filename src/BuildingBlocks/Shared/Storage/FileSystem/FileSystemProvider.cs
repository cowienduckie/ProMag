using Microsoft.Extensions.Options;

namespace Shared.Storage.FileSystem;

public class FileSystemStorageProvider : IStorageProvider
{
    private readonly IOptions<FileSystemOptions> _options;

    public FileSystemStorageProvider(IOptions<FileSystemOptions> options)
    {
        _options = options;
    }

    public string Name => "FileSystem";

    public IStore BuildStore(string storeName, IStorageStoreOptions storeOptions)
    {
        return new FileSystemStore(storeName, storeOptions.Parameters["Path"], _options.Value.RootPath);
    }
}