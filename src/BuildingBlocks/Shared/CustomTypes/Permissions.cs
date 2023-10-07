namespace Shared.CustomTypes;

public struct Permissions
{
    public const string PERMISSION_CLAIM_TYPE = "permission";

    //MANAGE_PROFILE = 0
    public const string PROFILE_FULL = "0.f";
    public const string PROFILE_VIEW = "0.v";

    //MANAGE_USER = 1
    public const string USER_FULL = "1.f";
    public const string USER_VIEW = "1.v";
    public const string USER_CREATE = "1.c";

    public const string USER_DELETE = "1.d";

    // MANAGE_PERSON = 2
    public const string PERSON_FULL = "2.f";
    public const string PERSON_VIEW = "2.v";
    public const string PERSON_CREATE = "2.c";
    public const string PERSON_DELETE = "2.d";

    // MANAGE_CONTACT = 3
    public const string CONTACT_FULL = "3.f";
    public const string CONTACT_VIEW = "3.v";
    public const string CONTACT_CREATE = "3.c";
    public const string CONTACT_DELETE = "3.d";

    // MANAGE_ROLE = 4
    public const string ROLE_FULL = "4.f";
    public const string ROLE_VIEW = "4.v";
    public const string ROLE_CREATE = "4.c";
}