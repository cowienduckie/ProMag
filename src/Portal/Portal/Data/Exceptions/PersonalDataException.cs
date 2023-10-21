using Shared.CustomTypes;

namespace Portal.Data.Exceptions;

public class PortalException : CustomException
{
    public PortalException()
    {
    }

    public PortalException(string message)
        : base(message)
    {
    }

    public PortalException(string message, Exception innerException) : base(message, innerException)
    {
    }
}