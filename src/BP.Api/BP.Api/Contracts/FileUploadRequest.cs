using System.ComponentModel.DataAnnotations;

namespace BP.Api.Contracts;

public class FileUploadRequest
{
    [Required]
    public string SkuId { get; set; } = null!;

    [Required]
    public IFormFile File { get; set; } = null!;
}
