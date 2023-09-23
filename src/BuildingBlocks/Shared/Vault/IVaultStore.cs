namespace Shared.Vault;

public interface IVaultStore
{
    Task<IDictionary<string, object>> GetDefaultAsync();
}