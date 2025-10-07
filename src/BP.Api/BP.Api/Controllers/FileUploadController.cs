using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Shared.Types;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(IFileUploadService fileUploadService, ILogger<FileUploadController> logger) : BaseController(logger)
{
    [HttpPost("uploadproduct")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProductImage([FromForm] FileUploadRequest request)
    {
        var file = request.File;
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var fileUpload = new FileUpload
        {
            FileName = $"{request.SkuId}.jpg",
            ContentType = file.ContentType,
            Content = file.OpenReadStream()
        };

        var url = await fileUploadService.UploadAsync(fileUpload);
        return Ok(new { url });
    }
}
