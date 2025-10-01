namespace BP.Application.Interfaces.Services;

using BP.Application.Interfaces.SkuAttributes;

public interface IFeatureService
{
    IEnumerable<Feature> GetAllFeatures();
}