namespace BP.Shared.Types;

public record FileDownload
{
    public Stream Content { get; set; } = default!;
    public string ContentType { get; set; } = "application/octet-stream";
}
