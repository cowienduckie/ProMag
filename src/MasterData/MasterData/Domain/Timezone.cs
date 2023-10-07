namespace MasterData.Domain;

public class Timezone
{
    public Timezone()
    {
    }

    public Timezone(string shortName, string code, string displayName)
    {
        Code = code;
        ShortName = shortName;
        DisplayName = displayName;
    }

    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string ShortName { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
}