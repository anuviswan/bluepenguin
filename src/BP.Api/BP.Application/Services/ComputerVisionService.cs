using BP.Application.Interfaces.Options;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.Types;
using BP.Domain.Repository;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BP.Application.Services;

public class ComputerVisionService(
    HttpClient httpClient,
    IFileUploadService fileUploadService,
    IProductImageRepository productImageRepository,
    ILogger<IComputerVisionService> logger,
    IOptions<ComputerVisionOptions> visionOptions) : IComputerVisionService
{
    private ComputerVisionOptions VisionOptions => visionOptions.Value;

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
            content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            using var response = await httpClient.PostAsync(endpoint, content).ConfigureAwait(false);
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

    private static async Task<byte[]> ReadAllBytesAsync(Stream stream)
    {
        using var memory = new MemoryStream();
        await stream.CopyToAsync(memory).ConfigureAwait(false);
        return memory.ToArray();
    }
}
