using BP.Shared.SkuAttributes;

namespace BP.Application.Interfaces.Services;

public interface ICategoryService
{
    IEnumerable<Category> GetAllCategories();

}
