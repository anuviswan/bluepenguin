using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using BP.Shared.Types;
using System.Linq;

namespace BP.Application.Services;

public class ProductImageService(IFileUploadService fileUploadService, IProductImageRepository productImageRepository) : IProductImageService
{
    public async Task<FileDownload?> DownloadByImageIdAsync(string skuId,string imageId)
    {
        var metaInfo = await productImageRepository.GetProductImageById(skuId,imageId).ConfigureAwait(false);
        if (metaInfo == null) return null;
        var fileDownload = await fileUploadService.DownloadByBlobNameAsync(metaInfo.BlobName).ConfigureAwait(false);

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
        var metaInfos = await productImageRepository.GetProductImagesBySku(skuId).ConfigureAwait(false);
        var downloads = new List<FileDownload>();

        foreach (var meta in metaInfos)
        {
            var download = await fileUploadService.DownloadByBlobNameAsync(meta.BlobName).ConfigureAwait(false);
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
        var metaInfos = await productImageRepository.GetProductImagesBySku(skuId).ConfigureAwait(false);
        return metaInfos.Select(mi => mi.RowKey);
    }

    public async Task<string?> GetPrimaryImageIdForSkuId(string skuId)
    {
        var metaInfos = await productImageRepository.GetProductImagesBySku(skuId).ConfigureAwait(false);
        return metaInfos.FirstOrDefault(mi => mi.IsPrimary)?.RowKey;
    }

    public async Task<string?> GetPrimaryImageUrlForSkuId(string skuId)
    {
        var metaInfos = await productImageRepository.GetProductImagesBySku(skuId).ConfigureAwait(false);
        var primary = metaInfos.FirstOrDefault(mi => mi.IsPrimary);
        if (primary == null)
            return null;

        return await fileUploadService.GetBlobUrlAsync(primary.BlobName).ConfigureAwait(false);
    }

    public async Task<string> UploadAsync(FileUpload file, bool isPrimary = false)
    {
        var blobName = await fileUploadService.UploadAsync(file).ConfigureAwait(false);
        await productImageRepository.AddProductImage(new ProductImageEntity
        {
            PartitionKey = file.SkuId,
            RowKey = file.ImageId,
            BlobName = blobName,
            IsPrimary = isPrimary,
            ContentType = file.ContentType
        }).ConfigureAwait(false);

        return blobName;
    }


    public async Task<bool> SetPrimaryImageAsync(string skuId, string imageId)
    {
        var targetImage = await productImageRepository.GetProductImageById(skuId, imageId).ConfigureAwait(false);
        if (targetImage == null)
            return false;

        var imagesForSku = await productImageRepository.GetProductImagesBySku(skuId).ConfigureAwait(false);
        var currentPrimaryImage = imagesForSku.FirstOrDefault(image => image.IsPrimary && image.RowKey != imageId);

        if (currentPrimaryImage != null)
        {
            currentPrimaryImage.IsPrimary = false;
            await productImageRepository.UpdateProductImage(currentPrimaryImage).ConfigureAwait(false);
        }

        if (!targetImage.IsPrimary)
        {
            targetImage.IsPrimary = true;
            await productImageRepository.UpdateProductImage(targetImage).ConfigureAwait(false);
        }

        return true;
    }

    public async Task<bool> DeleteProductImageAsync(string skuId, string imageId)
    {
        return await productImageRepository.DeleteProductImage(skuId, imageId).ConfigureAwait(false);
    }
}
