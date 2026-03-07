using BP.Application.Interfaces.Types;
using BP.Shared.Types;
using Microsoft.Extensions.Logging;

namespace BP.Application.Interfaces.Services;

public interface IComputerVisionService
{
    Task<EmbeddingGenerationResult> GenerateEmbeddingsForAllImagesAsync(bool force, int maxConcurrency = 5);
}
