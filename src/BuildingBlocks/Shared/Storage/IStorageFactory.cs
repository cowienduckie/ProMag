namespace Shared.Storage;

public interface IStorageFactory
{
    IStore? GetStore(string storeName);
}