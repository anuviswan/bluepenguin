using System.ComponentModel;

namespace BP.Application.Interfaces.SkuAttributes;

public enum Collection
{
    [Description("Onam")]
    ONM,

    [Description("Christmas")]
    CMS,

    [Description("Ocean")]
    OCN,

    [Description("Nature")]
    NAT,

    [Description("Traditional")]
    TRD,

    [Description("Spotlight")]
    SLT,

    [Description("Signature")]
    SGN,

    [Description("Vintage")]
    VIN
}
