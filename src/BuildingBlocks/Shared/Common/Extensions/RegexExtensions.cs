using System.Text.RegularExpressions;

namespace Shared.Common.Extensions;

public static partial class RegexExtensions
{
    [GeneratedRegex("\\s+")]
    private static partial Regex WhiteSpace();

    public static string ReplaceWhitespace(this string input, string replacement)
    {
        return WhiteSpace().Replace(input, replacement);
    }
}