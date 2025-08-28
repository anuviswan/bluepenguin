using System.ComponentModel;

namespace BP.Application.Interfaces.SkuAttributes;

public enum Material
{
    [Description("Resin")]
    RS,

    [Description("Clay")]
    CY,

    [Description("Bead")]
    BD
}
