using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using Shared.Common.Extensions;

namespace Shared.Common.ApiResponse;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public enum Errors
{
    [Description("Interal server error. Please contact the administrator.")]
    COM_000,

    [Description("Not found {CustomMessage}.")]
    COM_001
}

public static class ErrorExtensions
{
    public static string GetMessages(this Errors error, params string[] messageParams)
    {
        var messageFormat = error.GetDescription();
        var paramsCount = messageFormat.CountUniqueParams();

        if (paramsCount != messageParams.Length)
        {
            throw new ArgumentException(
                "The number of parameters in the error message does not match the number of parameters passed.");
        }

        return string.Format(messageFormat, messageParams.ToArray<object>());
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