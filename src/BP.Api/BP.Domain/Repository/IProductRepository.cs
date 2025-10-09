using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IProductRepository : IGenericRepository<ProductEntity>
{
    public Task<IEnumerable<ProductEntity>> GetProductsByCategory(string categoryCode, int yearCode);
}
