using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using BP.Shared.Types;

namespace BP.Application.Services;

public class ProductImageService(IFileUploadService fileUploadService,IProductImageRepository productImageRepository) : IProductImageService
{
    public Task<FileDownload?> DownloadAsync(string blobName)
    {
        throw new NotImplementedException();
    }

    public async Task<string> UploadAsync(FileUpload file, bool isPrimary = false)
    {
        var blobName = await fileUploadService.UploadAsync(file);
        await productImageRepository.AddProductImage(new ProductImageEntity
        {
            PartitionKey = file.SkuId,
            RowKey = file.ImageId,
            BlobName = blobName,
            IsPrimary = isPrimary,
            ContentType = file.ContentType,
            Extension = file.Extension ?? ".jpg",
        });

        return blobName;
    }
}
