namespace BP.Application.Interfaces.Services;
using BP.Shared.SkuAttributes;

public interface IMaterialService
{
    IEnumerable<Material> GetAllMaterials();
}
