namespace Shared.Storage.FileSystem;

public class FileSystemOptions
{
    public FileSystemOptions(string rootPath)
    {
        RootPath = rootPath;
    }

    public string RootPath { get; set; }
}