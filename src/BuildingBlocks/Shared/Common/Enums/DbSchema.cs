using System.ComponentModel;

namespace Shared.Common.Enums;

public enum DbSchema
{
    [Description("Identity")]
    Identity = 1,

    [Description("Master")]
    MasterData = 2,

    [Description("Personal")]
    PersonalData = 3
}