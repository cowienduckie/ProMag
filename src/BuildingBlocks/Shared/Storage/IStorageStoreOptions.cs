namespace Shared.Storage;

public interface IStorageStoreOptions
{
    string Provider { get; }
    Dictionary<string, string> Parameters { get; }
}