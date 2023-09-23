namespace Shared.Storage;

public interface IStore
{
    string Name { get; }

    Task InitAsync();
    Task<Stream> ReadAsync(IPrivateFileReference file);
    Task<byte[]> ReadAllBytesAsync(IPrivateFileReference file);
    Task<string> ReadAllTextAsync(IPrivateFileReference file);
    Task<string> SaveAsync(byte[] data, IPrivateFileReference file, IDictionary<string, string>? customMetaData, string contentType);
    Task<string> SaveAsync(Stream data, IPrivateFileReference file, IDictionary<string, string>? customMetaData, string contentType);
}