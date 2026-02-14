using BP.Shared.Types;

namespace BP.Application.Interfaces.Services;

public interface IProductImageService
{
    // Uploads a product image and returns the image ID.
    Task<string> UploadAsync(FileUpload file, bool isPrimary = false);

    // Downloads a product image by SKU ID and image ID.
    Task<FileDownload?> DownloadByImageIdAsync(string skuId, string imageId);

    // Downloads all product images associated with a SKU ID.
    Task<IEnumerable<FileDownload>> DownloadBySkuIdAsync(string skuId);

    // Retrieves all image IDs associated with a SKU ID.
    Task<IEnumerable<string>> GetImageIdsForSkuId(string skuId);

    // Retrieves the primary image Id for a given SKU ID.
    Task<string?> GetPrimaryImageIdForSkuId(string skuId);

    // Deletes an image from a product by SKU ID and image ID.
    Task<bool> DeleteProductImageAsync(string skuId, string imageId);
}
