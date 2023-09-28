using System.ComponentModel;

namespace Shared.Common.Enums;

public enum DbSchema
{
    [Description("public")]
    Public = 0,

    [Description("identity")]
    Identity = 1,

    [Description("master")]
    MasterData = 2,

    [Description("auditing")]
    Auditing = 3
}