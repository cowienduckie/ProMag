using System.Text.RegularExpressions;

namespace Shared.Common.Helpers;

public static partial class RegexHelper
{
    [GeneratedRegex("\\s+")]
    private static partial Regex WhiteSpace();

    public static string ReplaceWhitespace(this string input, string replacement)
    {
        return WhiteSpace().Replace(input, replacement);
    }
}