namespace Shared.Storage.Internal;

public class PrivateFileReference : IPrivateFileReference
{
    public PrivateFileReference(string path)
    {
        Path = path.Replace("\\", "/").TrimStart('/');
    }

    public string Path { get; }
}