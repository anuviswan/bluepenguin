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

    [HttpGet("downloadByimageId")]
    public async Task<IActionResult> DownloadFileByImageId([FromQuery] string skuId, [FromQuery] string imageId)
    {
        if (string.IsNullOrEmpty(skuId) && string.IsNullOrEmpty(imageId))
            return BadRequest("Blob name is required.");

        var fileDownload = await productImageService.DownloadByImageIdAsync(skuId,imageId);
        if (fileDownload == null)
            return NotFound("File not found.");
        return File(fileDownload.Content, fileDownload.ContentType, Path.GetFileName(skuId));
    }

    [HttpGet("getAllImagesForSkuId")]
    public async Task<IActionResult> GetAllImagesForSkuId([FromQuery] string skuId)
    {
        if (string.IsNullOrEmpty(skuId))
            return BadRequest("SkuId is required.");
        var files = await productImageService.GetImageIdsForSkuId(skuId);
        if (files == null || !files.Any())
            return NotFound("No files found.");

        return Ok(files);
    }

}
