using BP.Application.Interfaces.Services;
using BP.Shared.Types;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(IFileUploadService fileUploadService, ILogger<FileUploadController> logger) : BaseController(logger)
{
    [HttpPost("product-image")]
    public async Task<IActionResult> UploadProductImage([FromForm] IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded.");

        var fileUpload = new FileUpload
        {
            FileName = file.FileName,
            ContentType = file.ContentType,
            Content = file.OpenReadStream()
        };

        var url = await fileUploadService.UploadAsync(fileUpload);
        return Ok(new { url });
    }
}
