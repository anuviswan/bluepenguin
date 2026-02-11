using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Shared.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(IProductImageService productImageService, ILogger<FileUploadController> logger) : BaseController(logger)
{
    /// <summary>
    /// Upload Product Image
    /// </summary>
    /// <param name="request">The request payload containing <c>Image</c> and <c>SkuId</c>.
    /// <param name="isPrimaryImage">Indicates whether this image should be treated as the primary image
    /// for the product identified in <c>request.SkuId</c>.
    /// <returns></returns>
    [HttpPost("uploadproduct")]
    [Consumes("multipart/form-data")]
    [Authorize]
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

    
    /// <summary>
    /// Retrieves and downloads a file associated with the specified image identifier and SKU identifier.
    /// </summary>
    /// <remarks>Returns a file download response if the image is found for the given SKU and image
    /// identifiers. If either parameter is missing or the file does not exist, an appropriate error response is
    /// returned.</remarks>
    /// <param name="skuId">The SKU identifier used to locate the product image file. Cannot be null or empty.</param>
    /// <param name="imageId">The unique identifier of the image to download. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> that contains the file to download if found; otherwise, a Bad Request or Not
    /// Found result.</returns>
    [HttpGet("downloadByimageId")]
    public async Task<IActionResult> DownloadFileByImageId([FromQuery] string skuId, [FromQuery] string imageId)
    {
        if (string.IsNullOrEmpty(skuId) || string.IsNullOrEmpty(imageId))
            return BadRequest("Blob name is required.");

        var fileDownload = await productImageService.DownloadByImageIdAsync(skuId,imageId);
        if (fileDownload == null)
            return NotFound("File not found.");
        return File(fileDownload.Content, fileDownload.ContentType, Path.GetFileName(skuId));
    }

    /// <summary>
    /// Retrieves all image identifiers associated with the specified SKU ID.
    /// </summary>
    /// <param name="skuId">The unique identifier of the SKU for which to retrieve image IDs. Cannot be null or empty.</param>
    /// <returns>An <see cref="IActionResult"/> containing a collection of image IDs if found; otherwise, a response indicating
    /// that no files were found or that the SKU ID is required.</returns>
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

    /// <summary>
    /// Retrieves the primary image identifier associated with the specified SKU identifier.
    /// </summary>
    /// <param name="skuId">The unique identifier of the SKU for which to retrieve the primary image ID. Cannot be null or empty.</param>
    /// <returns>An HTTP 200 response containing the primary image ID if found; otherwise, an HTTP 404 response if no primary
    /// image exists for the specified SKU, or an HTTP 400 response if the SKU identifier is missing.</returns>
    [HttpGet("getPrimaryImageIdForSkuId")]
    public async Task<IActionResult> GetPrimaryImageIdForSkuId([FromQuery] string skuId)
    {
        if (string.IsNullOrEmpty(skuId))
            return BadRequest("SkuId is required.");
        var imageId = await productImageService.GetPrimaryImageIdForSkuId(skuId);
        if (imageId == null)
            return NotFound("No primary image found.");
        return Ok(imageId);
    }

}
