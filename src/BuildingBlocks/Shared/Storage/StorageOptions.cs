namespace Shared.Storage;

public class StorageOptions
{
    public Dictionary<string, StorageStoreOptions> Stores { get; set; } = default!;

    public class StorageStoreOptions : IStorageStoreOptions
    {
        public string Provider { get; set; } = default!;

        public Dictionary<string, string> Parameters { get; set; } = default!;
    }
}