namespace BP.Application.Interfaces.Services;
using BP.Application.Interfaces.SkuAttributes;

public interface ICategoryService
{
    IEnumerable<Category> GetAllCategories();

}
