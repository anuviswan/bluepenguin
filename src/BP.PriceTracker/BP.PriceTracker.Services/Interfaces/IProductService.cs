using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Interfaces;

public interface IProductService
{
    Task<IEnumerable<Category>> GetCategoriesAsync();
}
