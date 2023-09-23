namespace Shared.Storage;

public class StorageOptions
{
    public StorageOptions(Dictionary<string, StorageStoreOptions> stores)
    {
        Stores = stores;
    }

    public Dictionary<string, StorageStoreOptions> Stores { get; set; }

    public class StorageStoreOptions : IStorageStoreOptions
    {
        public StorageStoreOptions(string provider, Dictionary<string, string> parameters)
        {
            Provider = provider;
            Parameters = parameters;
        }

        public string Provider { get; set; }

        public Dictionary<string, string> Parameters { get; set; }
    }
}