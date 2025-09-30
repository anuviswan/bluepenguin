using BP.Application.Interfaces.Services;
using BP.Shared.SkuAttributes;

namespace BP.Application.Services;

public class FeatureService : IFeatureService
{
    public IEnumerable<Feature> GetAllFeatures()
    {
        return Enum.GetValues(typeof(Feature)).Cast<Feature>();
    }
}
