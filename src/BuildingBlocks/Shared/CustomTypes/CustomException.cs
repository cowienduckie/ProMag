namespace Shared.CustomTypes;

public class CustomException : Exception
{
    public CustomException()
    {
    }

    public CustomException(string message, Exception? inner = null)
        : base(message, inner)
    {
    }
}