namespace Shared.Storage.FileSystem.Internal;

public class FileSystemFileReference : IFileReference
{
    public FileSystemFileReference(string filePath, string path)
    {
        FileSystemPath = filePath;
        Path = path.Replace('\\', '/');
    }

    public string FileSystemPath { get; }

    public IDictionary<string, string> CustomMetadata => throw new NotImplementedException();
    public string Path { get; }

    public Task<Stream> ReadAsync()
    {
        Stream stream = File.OpenRead(FileSystemPath);

        return Task.FromResult(stream);
    }

    public Task<string> ReadAllTextAsync()
    {
        return Task.FromResult(File.ReadAllText(FileSystemPath));
    }

    public Task<byte[]> ReadAllBytesAsync()
    {
        return Task.FromResult(File.ReadAllBytes(FileSystemPath));
    }

    public Task DeleteAsync()
    {
        File.Delete(FileSystemPath);

        return Task.FromResult(true);
    }
}