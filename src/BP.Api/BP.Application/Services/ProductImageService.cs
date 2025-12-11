using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using BP.Shared.Types;

namespace BP.Application.Services;

public class ProductImageService(IFileUploadService fileUploadService, IProductImageRepository productImageRepository) : IProductImageService
{
    public async Task<FileDownload?> DownloadByImageIdAsync(string skuId,string imageId)
    {
        var metaInfo = await productImageRepository.GetProductImageById(skuId,imageId);
        if (metaInfo == null) return null;
        var fileDownload = await fileUploadService.DownloadByBlobNameAsync(metaInfo.BlobName);

        if(fileDownload == null) 
            return null;

        return new FileDownload
        {
            Content = fileDownload.Content,
            ContentType = metaInfo.ContentType
        };
    }

    public async Task<IEnumerable<FileDownload>> DownloadBySkuIdAsync(string skuId)
    {
        var metaInfos = await productImageRepository.GetProductImagesBySku(skuId);
        var downloads = new List<FileDownload>();

        foreach (var meta in metaInfos)
        {
            var download = await fileUploadService.DownloadByBlobNameAsync(meta.BlobName);
            if (download != null)
            {
                downloads.Add(new FileDownload
                {
                    Content = download.Content,
                    ContentType = meta.ContentType
                });
            }
        }
        return downloads;
    }

    public async Task<IEnumerable<string>> GetImageIdsForSkuId(string skuId)
    {
        var metaInfos = await productImageRepository.GetProductImagesBySku(skuId);
        return metaInfos.Select(mi => mi.RowKey);
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
            ContentType = file.ContentType
        });

        return blobName;
    }
}
