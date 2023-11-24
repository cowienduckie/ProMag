using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Shared.Common.Helpers;

namespace Shared.Common.ApiResponse;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Errors
{
    [Description("Interal server error. Please contact the administrator.")]
    COM_000,

    [Description("Not found {0}.")]
    COM_001,

    [Description("User ID is not a valid Guid.")]
    VAL_000,

    [Description("Validation failed. Detail messages: \n{0}")]
    VAL_001
}

public static class ErrorExtensions
{
    public static string GetMessages(this Errors error, params object?[] messageParams)
    {
        var messageFormat = error
            .GetDescription()
            .Replace("\n", Environment.NewLine);
        var paramsCount = messageFormat.CountUniqueParams();

        if (paramsCount != messageParams.Length)
        {
            throw new ArgumentException(
                "The number of parameters in the error message does not match the number of parameters passed.");
        }

        messageParams = messageParams.Select(p => p?.ToString()).ToArray<object?>();

        return string.Format(messageFormat, messageParams);
    }

    public static string GetCode(this Errors error)
    {
        if (!Enum.IsDefined(typeof(Errors), error))
        {
            throw new InvalidEnumArgumentException(nameof(error), (int)error, typeof(Errors));
        }

        return nameof(error);
    }
}