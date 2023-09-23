using System.Collections.ObjectModel;

namespace Shared.CustomTypes;

public struct Actions
{
    public const string FULL = "f";
    public const string VIEW = "v";
    public const string CREATE = "c";
    public const string DELETE = "d";
}

public struct Resources
{
    public const string PROFILE = "0";
    public const string USER = "1";
    public const string PERSON = "2";
    public const string CONTACT = "3";
    public const string ROLE = "4";
}

public static class Permissions
{
    private static readonly Permission[] _all =
    {
        new(Resources.PROFILE, Actions.FULL),
        new(Resources.PROFILE, Actions.VIEW),
        new(Resources.USER, Actions.FULL),
        new(Resources.USER, Actions.VIEW),
        new(Resources.USER, Actions.CREATE),
        new(Resources.USER, Actions.DELETE),
        new(Resources.PERSON, Actions.FULL),
        new(Resources.PERSON, Actions.VIEW),
        new(Resources.PERSON, Actions.CREATE),
        new(Resources.PERSON, Actions.DELETE),
        new(Resources.CONTACT, Actions.FULL),
        new(Resources.CONTACT, Actions.VIEW),
        new(Resources.CONTACT, Actions.CREATE),
        new(Resources.CONTACT, Actions.DELETE),
        new(Resources.ROLE, Actions.FULL),
        new(Resources.ROLE, Actions.VIEW),
        new(Resources.ROLE, Actions.CREATE)
    };

    public static IReadOnlyList<Permission> All => new ReadOnlyCollection<Permission>(_all);
}

public record Permission(string Resource, string Action)
{
    public string Name => NameFor(Resource, Action);

    private static string NameFor(string resource, string action)
    {
        return $"{resource}.{action}";
    }
}