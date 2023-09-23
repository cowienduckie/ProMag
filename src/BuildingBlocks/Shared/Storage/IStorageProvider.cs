namespace Shared.Storage;

public interface IStorageProvider
{
    string Name { get; }
    IStore BuildStore(string storeName, IStorageStoreOptions storeOptions);
}