using Shared.Storage.Internal;

namespace Shared.Storage.Extensions;

public static class StoreExtensions
{
    public static Task<byte[]> ReadAllBytesAsync(this IStore store, string path)
    {
        return store.ReadAllBytesAsync(new PrivateFileReference(path));
    }

    public static Task<Stream> ReadAsync(this IStore store, string path)
    {
        return store.ReadAsync(new PrivateFileReference(path));
    }

    public static Task<string> ReadAllTextAsync(this IStore store, string path)
    {
        return store.ReadAllTextAsync(new PrivateFileReference(path));
    }

    public static Task<string> SaveAsync(this IStore store, Stream data, string path, IDictionary<string, string>? customMetaData = null,
        string contentType = "text/plain")
    {
        return store.SaveAsync(data, new PrivateFileReference(path), customMetaData, contentType);
    }

    public static Task<string> SaveAsync(this IStore store, byte[] data, string path, string contentType = "text/plain")
    {
        return store.SaveAsync(data, new PrivateFileReference(path), null, contentType);
    }
}