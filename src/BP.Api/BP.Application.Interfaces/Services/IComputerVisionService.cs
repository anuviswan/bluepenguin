using BP.Application.Interfaces.Types;

namespace BP.Application.Interfaces.Services;

public interface IComputerVisionService
{
    Task<EmbeddingGenerationResult> GenerateEmbeddingsForAllImagesAsync(bool force, int maxConcurrency = 5);
    Task<IEnumerable<ImageSimilarityMatch>> FindClosestProductImagesAsync(Stream imageStream, int limit = 5);
}
