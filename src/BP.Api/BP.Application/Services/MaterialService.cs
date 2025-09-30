using BP.Application.Interfaces.Services;
using BP.Shared.SkuAttributes;

namespace BP.Application.Services;

public class MaterialService : IMaterialService
{
    public IEnumerable<Material> GetAllMaterials()
    {
        return Enum.GetValues(typeof(Material)).Cast<Material>();
    }
}
