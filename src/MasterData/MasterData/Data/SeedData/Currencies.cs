using System.Collections.Immutable;
using MasterData.Domain;

namespace MasterData.Data.SeedData;

public static class Currencies
{
    public static readonly IImmutableList<Currency> Seeds = ImmutableArray.Create(
        new Currency("USD", "US Dollar", "$"),
        new Currency("VND", "Vietnamese Dong", "VND"),
        new Currency("EUR", "Euro Dollar", "€")
    );
}