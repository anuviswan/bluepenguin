namespace BP.Application.Interfaces.Types;

public record EmbeddingGenerationResult(
    int TotalScanned,
    int EmbeddingsGenerated,
    int Skipped,
    int Failures
);