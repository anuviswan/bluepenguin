using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;

namespace BP.Application.Services;

public class CategoryService : ICategoryService
{
    public IEnumerable<Category> GetAllCategories()
    {
        return Enum.GetValues(typeof(Category)).Cast<Category>();
    }
}
