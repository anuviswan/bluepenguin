using BP.Shared.Types;
using Microsoft.Extensions.Logging;

namespace BP.Application.Interfaces.Services;

public interface IProductImageService
{
    Task<string> UploadAsync(FileUpload file, bool isPrimary = false);
    Task<FileDownload?> DownloadByImageIdAsync(string skuId, string imageId);
    Task<IEnumerable<FileDownload>> DownloadBySkuIdAsync(string skuId);
    Task<IEnumerable<string>> GetImageIdsForSkuId(string skuId);
    Task<string?> GetPrimaryImageIdForSkuId(string skuId);
    Task<string?> GetPrimaryImageUrlForSkuId(string skuId);
    Task<string?> GetImageUrlForImageIdAsync(string skuId, string imageId);
    Task<bool> SetPrimaryImageAsync(string skuId, string imageId);
    Task<bool> DeleteProductImageAsync(string skuId, string imageId);
    // New method for generating embeddings
    Task<EmbeddingGenerationResult> GenerateEmbeddingsForAllImagesAsync(bool force, ILogger logger, int maxConcurrency = 5);
}

// Result DTO for embedding generation
public record EmbeddingGenerationResult(
    int TotalScanned,
    int EmbeddingsGenerated,
    int Skipped,
    int Failures
);
