using Shared.CustomTypes;

namespace Shared.Vault;

public class VaultException : CustomException
{
    public VaultException(string key) : this(null, key)
    {
    }

    public VaultException(Exception? inner, string key) : this(string.Empty, inner, key)
    {
    }

    public VaultException(string message, Exception? inner, string key) : base(message, inner)
    {
        Key = key;
    }

    public string Key { get; private set; }
}