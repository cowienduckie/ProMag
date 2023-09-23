namespace Shared.Storage;

public interface IFileReference : IPrivateFileReference
{
    Task<Stream> ReadAsync();
    Task<string> ReadAllTextAsync();
    Task<byte[]> ReadAllBytesAsync();
    Task DeleteAsync();
}