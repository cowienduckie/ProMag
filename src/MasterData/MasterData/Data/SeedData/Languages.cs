using System.Collections.Immutable;
using MasterData.Domain;

namespace MasterData.Data.SeedData;

public static class Languages
{
    public static readonly IImmutableList<Language> Seeds = ImmutableArray.Create(
        new Language("en-US", "English (United States)"),
        new Language("vi-VN", "Vietnamese (Vietnam)"),
        new Language("fr-FR", "French (France)"),
        new Language("de-DE", "German (Germany)"),
        new Language("ja-JP", "Japanese (Japan)"),
        new Language("zh-CN", "Chinese (RPC)")
    );
}