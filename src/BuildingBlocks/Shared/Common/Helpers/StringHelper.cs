using System.Text.RegularExpressions;

namespace Shared.Common.Helpers;

public static partial class StringHelper
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

    public static int CountUniqueParams(this string s)
    {
        var matches = StringParamRegex().Matches(s);

        return matches.Select(m => m.Value).Distinct().Count();
    }

    public static int CountParams(this string s)
    {
        var matches = StringParamRegex().Matches(s);

        return matches.Count;
    }

    [GeneratedRegex("{(.*?)}")]
    private static partial Regex StringParamRegex();
}