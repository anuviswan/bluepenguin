namespace BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;

public interface IMaterialService
{
    IEnumerable<Material> GetAllMaterials();
}
