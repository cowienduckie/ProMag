namespace MasterData.Domain;

public class Currency
{
    public Currency()
    {
    }

    public Currency(string code, string displayName, string symbol)
    {
        Code = code;
        DisplayName = displayName;
        Symbol = symbol;
    }

    public int Id { get; set; }
    public string Code { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Symbol { get; set; } = default!;
}