namespace Shared.ValidationModels;

public struct ErrorCodes
{
    public const string DUPLICATE_EMAIL = "DuplicateEmail";
    public const string GENERAL_ERROR = "GeneralError";
    public const string DATA_NOTFOUND = "DataNotFound";
}

public class ValidationError
{
    public ValidationError(string message, string field = "", string code = "")
    {
        Code = string.IsNullOrEmpty(code) ? code : ErrorCodes.GENERAL_ERROR;
        Field = string.IsNullOrEmpty(field) ? field : null;
        Message = message;
    }

    public string Code { get; private set; }
    public string? Field { get; private set; }
    public string Message { get; private set; }
}