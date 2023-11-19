using System.ComponentModel;

namespace Shared.Common.ApiResponse;

public enum ResponseStatuses
{
    Success = 200,
    BadRequest = 400,
    Unauthorized = 401,
    Forbidden = 403,
    NotFound = 404,
    MethodNotAllowed = 405,
    Fail = 500,
    FailWithException = 511
}

public static class ResponseStatusExtensions
{
    public static string GetName(this ResponseStatuses status)
    {
        if (!Enum.IsDefined(typeof(ResponseStatuses), status))
        {
            throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(ResponseStatuses));
        }

        return nameof(status);
    }

    public static int GetCode(this ResponseStatuses status)
    {
        if (!Enum.IsDefined(typeof(ResponseStatuses), status))
        {
            throw new InvalidEnumArgumentException(nameof(status), (int)status, typeof(ResponseStatuses));
        }

        return (int)status;
    }
}