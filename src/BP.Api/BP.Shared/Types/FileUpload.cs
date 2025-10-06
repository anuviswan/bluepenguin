namespace BP.Shared.Types;

public record FileUpload
{
    public string FileName { get; init; } = default!;
    public string ContentType { get; init; } = default!;
    public Stream Content { get; init; } = default!;
}
