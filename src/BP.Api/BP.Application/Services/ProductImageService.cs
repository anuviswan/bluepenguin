using BP.Application.Interfaces.Options;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using BP.Domain.Repository;
using BP.Shared.Types;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;

namespace BP.Application.Services;

public class ProductImageService(
        HttpClient httpClient,
        IFileUploadService fileUploadService,
        IProductImageRepository productImageRepository,
        ILogger<IProductImageService> logger,
        IOptions<ComputerVisionOptions> visionOptions) : IProductImageService
{
    private ComputerVisionOptions VisionOptions => visionOptions.Value;

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


    public async Task<IEnumerable<ImageSimilarityMatch>> FindClosestProductImagesAsync(Stream imageStream, int limit = 5)
    {
        if (imageStream == null)
            throw new ArgumentNullException(nameof(imageStream));

        if (limit <= 0)
            limit = 5;

        var queryEmbedding = await GenerateEmbeddingFromAzureAsync(imageStream).ConfigureAwait(false);
        if (queryEmbedding == null || queryEmbedding.Length == 0)
            return Enumerable.Empty<ImageSimilarityMatch>();

        var allImages = await productImageRepository.GetAllProductImagesAsync().ConfigureAwait(false);

        var matches = new List<ImageSimilarityMatch>();
        foreach (var image in allImages)
        {
            if (string.IsNullOrWhiteSpace(image.Embedding))
                continue;

            float[]? candidateEmbedding;
            try
            {
                candidateEmbedding = JsonSerializer.Deserialize<float[]>(image.Embedding);
            }
            catch (JsonException)
            {
                continue;
            }

            if (candidateEmbedding == null || candidateEmbedding.Length == 0 || candidateEmbedding.Length != queryEmbedding.Length)
                continue;

            var similarity = CalculateCosineSimilarity(queryEmbedding, candidateEmbedding);
            matches.Add(new ImageSimilarityMatch(
                image.PartitionKey,
                image.RowKey,
                image.BlobName,
                similarity
            ));
        }

        return matches
            .OrderByDescending(m => m.Similarity)
            .Take(limit);
    }

    public async Task<EmbeddingGenerationResult> GenerateEmbeddingsForAllImagesAsync(bool force, int maxConcurrency = 5)
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

                    var fileDownload = await fileUploadService.DownloadByBlobNameAsync(image.BlobName).ConfigureAwait(false);

                    if (fileDownload is null) return;


                    if (string.IsNullOrEmpty(url))
                    {
                        logger.LogWarning($"No blob URL for image {image.BlobName}");
                        Interlocked.Increment(ref failed);
                        return;
                    }
                    var embedding = await GenerateEmbeddingFromAzureAsync(fileDownload.Content).ConfigureAwait(false);
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

    private async Task<float[]?> GenerateEmbeddingFromAzureAsync(Stream imageStream)
    {
        var endpoint = $"{VisionOptions.Url.TrimEnd('/')}/computervision/retrieval:vectorizeImage?model-version={VisionOptions.ModelVersion}&api-version={VisionOptions.ApiVersion}";

        try
        {
            if (imageStream.CanSeek)
                imageStream.Position = 0;

            var bytes = await ReadAllBytesAsync(imageStream);

            using var content = new ByteArrayContent(bytes);
            content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");

            using var response =
                await httpClient.PostAsync(endpoint, content).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning($"Vision API failed: {response.StatusCode}");
                return null;
            }
            using var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            using var doc = await JsonDocument.ParseAsync(stream).ConfigureAwait(false);
            if (doc.RootElement.TryGetProperty("vector", out var embeddingElement) && embeddingElement.ValueKind == JsonValueKind.Array)
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


    private static double CalculateCosineSimilarity(float[] source, float[] target)
    {
        double dotProduct = 0;
        double sourceMagnitude = 0;
        double targetMagnitude = 0;

        for (var i = 0; i < source.Length; i++)
        {
            dotProduct += source[i] * target[i];
            sourceMagnitude += source[i] * source[i];
            targetMagnitude += target[i] * target[i];
        }

        if (sourceMagnitude <= 0 || targetMagnitude <= 0)
            return 0;

        return dotProduct / (Math.Sqrt(sourceMagnitude) * Math.Sqrt(targetMagnitude));
    }

    private static async Task<byte[]> ReadAllBytesAsync(Stream stream)
    {
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory).ConfigureAwait(false);
        return memory.ToArray();
    }
}


