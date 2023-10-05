using Shared.CustomTypes;

namespace PersonalData.Data.Exceptions;

public class PersonalDataException : CustomException
{
    public PersonalDataException()
    {
    }

    public PersonalDataException(string message)
        : base(message)
    {
    }

    public PersonalDataException(string message, Exception innerException) : base(message, innerException)
    {
    }
}