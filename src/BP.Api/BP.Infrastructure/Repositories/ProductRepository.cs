using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Infrastructure.Repositories;

public class ProductRepository : GenericRepository<Product>, IProductRepository
{
    public IEnumerable<Product> GetProductsByCategory(string categoryId)
    {
        throw new NotImplementedException();
    }
}
