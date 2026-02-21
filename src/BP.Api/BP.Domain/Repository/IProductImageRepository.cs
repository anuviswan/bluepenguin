using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IProductImageRepository
{
    Task<ProductImageEntity?> AddProductImage(ProductImageEntity image);
    Task<IEnumerable<ProductImageEntity>> GetProductImagesBySku(string sku);
    Task<ProductImageEntity?> GetProductImageById(string sku, string imageId);
    Task<ProductImageEntity?> UpdateProductImage(ProductImageEntity image);
    Task<bool> DeleteProductImage(string sku, string imageId);
    Task DeleteAllAsync();
}
