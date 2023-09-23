using Shared.CustomTypes;

namespace Shared.Vault;

public class VaultAuthTypeNotSupportedException : CustomException
{
    public VaultAuthTypeNotSupportedException(string authType) : this(string.Empty, authType)
    {
    }

    public VaultAuthTypeNotSupportedException(string message, string authType) : base(message)
    {
        AuthType = authType;
    }

    public string AuthType { get; private set; }
}