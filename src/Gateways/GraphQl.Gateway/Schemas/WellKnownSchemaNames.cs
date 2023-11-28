using Humanizer;

namespace GraphQl.Gateway.Schemas;

public static class WellKnownSchemaNames
{
    public static readonly string PersonalData = nameof(PersonalData).Camelize();
    public static readonly string MasterData = nameof(MasterData).Camelize();
    public static readonly string Portal = nameof(Portal).Camelize();
    public static readonly string Communication = nameof(Communication).Camelize();
}