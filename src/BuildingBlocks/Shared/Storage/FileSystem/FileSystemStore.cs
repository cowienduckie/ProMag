using Shared.Storage.FileSystem.Internal;

namespace Shared.Storage.FileSystem;

public class FileSystemStore : IStore
{
    public FileSystemStore(string storeName, string path, string rootPath)
    {
        Guard.NotNullOrEmpty(path);

        AbsolutePath = Path.IsPathRooted(path) ? path : Path.Combine(rootPath, path);
        Name = storeName;
    }

    internal string AbsolutePath { get; }

    public string Name { get; }

    public Task InitAsync()
    {
        if (!Directory.Exists(AbsolutePath))
        {
            Directory.CreateDirectory(AbsolutePath);
        }

        return Task.FromResult(0);
    }

    public async Task<byte[]> ReadAllBytesAsync(IPrivateFileReference file)
    {
        var fileReference = await InternalGetAsync(file);

        return await fileReference.ReadAllBytesAsync();
    }

    public async Task<Stream> ReadAsync(IPrivateFileReference file)
    {
        var fileReference = await InternalGetAsync(file);

        return await fileReference.ReadAsync();
    }

    public async Task<string> ReadAllTextAsync(IPrivateFileReference file)
    {
        var fileReference = await InternalGetAsync(file);

        return await fileReference.ReadAllTextAsync();
    }

    public async Task<string> SaveAsync(byte[] data, IPrivateFileReference file, IDictionary<string, string>? customMetaData, string contentType)
    {
        using var stream = new MemoryStream(data, 0, data.Length);

        return await SaveAsync(stream, file, customMetaData, contentType);
    }

    public async Task<string> SaveAsync(Stream data, IPrivateFileReference file, IDictionary<string, string>? customMetaData, string contentType)
    {
        var fileReference = await InternalGetAsync(file, false);

        EnsurePathExists(fileReference.FileSystemPath);

        await using (var fileStream = File.Open(fileReference.FileSystemPath, FileMode.Create, FileAccess.Write))
        {
            data.Position = 0;
            await data.CopyToAsync(fileStream);
        }

        return fileReference.FileSystemPath;
    }

    private async Task<FileSystemFileReference> InternalGetAsync(IPrivateFileReference file, bool checkIfExists = true)
    {
        if (file is FileSystemFileReference fileSystemFile)
        {
            return fileSystemFile;
        }

        return await InternalGetAsync(file.Path, checkIfExists);
    }

    private Task<FileSystemFileReference> InternalGetAsync(string path, bool checkIfExists = true)
    {
        var fullPath = Path.Combine(AbsolutePath, path);

        if (checkIfExists && !File.Exists(fullPath))
        {
            throw new FileNotFoundException();
        }

        return Task.FromResult(new FileSystemFileReference(fullPath, path, this));
    }

    private static void EnsurePathExists(string path)
    {
        var directoryPath = Path.GetDirectoryName(path);

        if (!Directory.Exists(directoryPath) && !string.IsNullOrEmpty(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
    }
}