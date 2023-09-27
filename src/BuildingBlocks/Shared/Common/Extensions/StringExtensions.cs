using System.Text.RegularExpressions;

namespace Shared.Common.Extensions;

public static class StringExtensions
{
    public static string RemoveSuffix(this string s, string suffix)
    {
        return Regex.Replace(s, suffix + "$", string.Empty);
    }

    public static string RemoveSuffix(this string s, char suffix)
    {
        return Regex.Replace(s, suffix + "$", string.Empty);
    }

    public static string RemoveSuffix(this string s, int numberOfCharacters)
    {
        return s.Substring(0, s.Length - numberOfCharacters);
    }

    public static string RemovePrefix(this string s, string prefix)
    {
        return Regex.Replace(s, "^" + prefix, string.Empty);
    }

    public static string RemovePrefix(this string s, char prefix)
    {
        return Regex.Replace(s, "^" + prefix, string.Empty);
    }

    public static string RemovePrefix(this string s, int numberOfCharacters)
    {
        return s.Substring(numberOfCharacters);
    }
}