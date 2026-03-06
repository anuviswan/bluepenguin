using BP.Application.Interfaces.Options;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using BP.Shared.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;

namespace BP.Application.Services;

public class ProductImageService : IProductImageService
{
    private readonly HttpClient httpClient;
    private readonly IFileUploadService fileUploadService;
    private readonly IProductImageRepository productImageRepository;
    private readonly ComputerVisionOptions visionOptions;

    public ProductImageService(
        HttpClient httpClient,
        IFileUploadService fileUploadService,
        IProductImageRepository productImageRepository,
        IOptions<ComputerVisionOptions> visionOptions)
    {
        this.httpClient = httpClient;
        this.fileUploadService = fileUploadService;
        this.productImageRepository = productImageRepository;
        this.visionOptions = visionOptions.Value;
    }

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

    public async Task<string?> GetImageUrlForImageIdAsync(string skuId, string imageId)
    {
        var metaInfo = await productImageRepository.GetProductImageById(skuId, imageId).ConfigureAwait(false);
        if (metaInfo == null)
            return null;

        return await fileUploadService.GetBlobUrlAsync(metaInfo.BlobName).ConfigureAwait(false);
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

    public async Task<EmbeddingGenerationResult> GenerateEmbeddingsForAllImagesAsync(bool force, ILogger logger, int maxConcurrency = 5)
    {
        var allImages = (await productImageRepository.GetAllProductImagesAsync().ConfigureAwait(false)).ToList();

        int total = 0, generated = 0, skipped = 0, failed = 0;
        var semaphore = new SemaphoreSlim(maxConcurrency);
        var tasks = new List<Task>();

        foreach (var image in allImages)
        {
            total++;
            if (!force && !string.IsNullOrEmpty(image.Embedding))
            {
                skipped++;
                continue;
            }
            await semaphore.WaitAsync().ConfigureAwait(false);
            tasks.Add(Task.Run(async () =>
            {
                try
                {
                    var url = await fileUploadService.GetBlobUrlAsync(image.BlobName).ConfigureAwait(false);
                    if (string.IsNullOrEmpty(url))
                    {
                        logger.LogWarning($"No blob URL for image {image.BlobName}");
                        Interlocked.Increment(ref failed);
                        return;
                    }
                    var embedding = await GenerateEmbeddingFromAzureAsync(url, logger).ConfigureAwait(false);
                    if (embedding != null)
                    {
                        image.Embedding = JsonSerializer.Serialize(embedding);
                        await productImageRepository.UpdateProductImage(image).ConfigureAwait(false);
                        Interlocked.Increment(ref generated);
                        logger.LogInformation($"Generated embedding for {image.BlobName}");
                    }
                    else
                    {
                        Interlocked.Increment(ref failed);
                        logger.LogWarning($"Failed to generate embedding for {image.BlobName}");
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"Error processing image {image.BlobName}");
                    Interlocked.Increment(ref failed);
                }
                finally
                {
                    semaphore.Release();
                }
            }));
        }
        await Task.WhenAll(tasks).ConfigureAwait(false);
        return new EmbeddingGenerationResult(total, generated, skipped, failed);
    }

    private async Task<float[]?> GenerateEmbeddingFromAzureAsync(string imageUrl, ILogger logger)
    {
        var endpoint = $"{visionOptions.Url.TrimEnd('/')}/retrieval:vectorizeImage?api-version=2023-04-01-preview";
        var request = new { url = imageUrl };
        try
        {
            using var response = await httpClient.PostAsJsonAsync(endpoint, request).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning($"Vision API failed: {response.StatusCode}");
                return null;
            }
            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            if (doc.RootElement.TryGetProperty("embedding", out var embeddingElement) && embeddingElement.ValueKind == JsonValueKind.Array)
            {
                var floats = new List<float>();
                foreach (var v in embeddingElement.EnumerateArray())
                {
                    if (v.TryGetSingle(out var f))
                        floats.Add(f);
                }
                return floats.ToArray();
            }
            logger.LogWarning("Vision API response missing 'embedding' array");
            return null;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Vision API call failed");
            return null;
        }
    }
}


