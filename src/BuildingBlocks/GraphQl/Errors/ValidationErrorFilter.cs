using Shared.ValidationModels;

namespace GraphQl.Errors;

public class ValidationErrorFilter : IErrorFilter
{
    public IError OnError(IError error)
    {
        if (error.Exception is ValidationException exception)
        {
            return error.WithException(exception);
        }

        return error;
    }
}