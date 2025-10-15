namespace BP.Shared.Types;

public record FileUpload
{
    public string ImageId { get; init; } = default!;
    public string SkuId { get; init; } = default!;
    public string ContentType { get; init; } = default!;
    public Stream Content { get; init; } = default!;
    public string? Extension { get; init; }
}
