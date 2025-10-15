using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Shared.Types;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(IProductImageService productImageService, ILogger<FileUploadController> logger) : BaseController(logger)
{
    [HttpPost("uploadproduct")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProductImage([FromForm] FileUploadRequest request,bool isPrimaryImage)
    {
        var file = request.File;
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        var fileUpload = new FileUpload
        {
            SkuId = request.SkuId,
            ImageId = Guid.NewGuid().ToString(),
            ContentType = file.ContentType,
            Content = file.OpenReadStream(),
            Extension = extension
        };

        var url = await productImageService.UploadAsync(fileUpload,isPrimaryImage);
        return Ok(new { url });
    }

    [HttpGet("download")]
    public async Task<IActionResult> DownloadFile([FromQuery] string blobName)
    {
        if (string.IsNullOrEmpty(blobName))
            return BadRequest("Blob name is required.");

        var fileDownload = await productImageService.DownloadAsync(blobName);
        if (fileDownload == null)
            return NotFound("File not found.");
        return File(fileDownload.Content, fileDownload.ContentType, Path.GetFileName(blobName));
    }
}
