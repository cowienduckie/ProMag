namespace Shared.Common.Helpers;

public static class DateTimeHelper
{
    public static DateTime ToDateTime(this object obj)
    {
        if (obj is DateTime dateTime)
        {
            return dateTime.ToUniversalTime();
        }

        if (obj is string stringValue && DateTime.TryParse(stringValue, out var parsedDateTime))
        {
            return parsedDateTime.ToUniversalTime();
        }

        throw new ArgumentException("Invalid DateTime format");
    }
}