using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IProductRepository : IGenericRepository<Product>
{
    public Task<IEnumerable<Product>> GetProductsByCategory(string categoryCode, int yearCode);
}
