namespace MasterData.Boundaries.GraphQl.Dtos;

public class CurrencyDto
{
    public string Code { get; set; } = default!;
    public string DisplayName { get; set; } = default!;
    public string Symbol { get; set; } = default!;
}