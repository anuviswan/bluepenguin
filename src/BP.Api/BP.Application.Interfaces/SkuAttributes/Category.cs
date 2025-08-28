using System.ComponentModel;

namespace BP.Application.Interfaces.SkuAttributes;

public enum Category
{
    [Description("Bracelets")]
    BR,

    [Description("Earings")]
    ER,

    [Description("Necklaces")]
    NK,

    [Description("Rings")]
    RI,

    [Description("Pendants")]
    PD,

    [Description("Home Decor")]
    HD,

    [Description("Watches & Clocks")]
    CL,

    [Description("Coasters")]
    CO,

    [Description("Bags")]
    BG,

    [Description("Key Chains")]
    KC,

    [Description("Preservation")]
    PR
}
