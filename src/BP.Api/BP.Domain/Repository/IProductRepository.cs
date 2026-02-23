using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IProductRepository : IGenericRepository<ProductEntity>
{
    public Task<IEnumerable<ProductEntity>> GetProductsByCategory(string categoryCode, int yearCode);
    public Task DeleteAllAsync();

    public Task<bool> CheckIfSkuExistsAsync(string sku);

    public Task<IEnumerable<TopCategoryStats>> GetTopCategoriesAsync(int count);

    public Task<IEnumerable<TopDiscountStats>> GetTopDiscountsAsync(int count);
}
