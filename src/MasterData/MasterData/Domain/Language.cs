namespace MasterData.Domain;

public class Language
{
    public Language()
    {
    }

    public Language(string code, string displayName)
    {
        Code = code;
        DisplayName = displayName;
    }

    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}