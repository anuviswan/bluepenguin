using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IProductRepository : IGenericRepository<Entities.Product>
{
    public IEnumerable<Product> GetProductsByCategory(string categoryId);
}
