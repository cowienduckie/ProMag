using System.ComponentModel;

namespace Shared.Common.Enums;

public enum DbSchema
{
    [Description("public")]
    Public = 0,

    [Description("Identity")]
    Identity = 1,

    [Description("Master")]
    MasterData = 2,

    [Description("Auditing")]
    Auditing = 3
}