namespace BP.Application.Interfaces.Services;
using BP.Shared.SkuAttributes;

public interface IFeatureService
{
    IEnumerable<Feature> GetAllFeatures();
}