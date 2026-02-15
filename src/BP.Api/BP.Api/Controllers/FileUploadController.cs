using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Shared.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileUploadController(
    IProductImageService productImageService,
    IProductService productService,
    ILogger<FileUploadController> logger) : BaseController(logger)
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
    [AllowAnonymous]
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
    [AllowAnonymous]
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
    /// Get detailed information about all images for a product.
    /// </summary>
    /// <remarks>
    /// Endpoint: GET <c>/api/fileupload/getproductimagesdetails</c>?skuId={skuId}
    /// Success: Returns 200 OK with a list of product image details including ImageId, BlobName, IsPrimary, ContentType, and DownloadUrl.
    /// Not Found: Returns 404 if no images exist for the product.
    /// Failure: Returns 400 Bad Request for invalid SKU or on exception.
    /// </remarks>
    /// <param name="skuId">The SKU of the product to retrieve images for.</param>
    /// <returns>List of detailed image information or an error result.</returns>
    [HttpGet("getproductimagesdetails")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProductImagesDetails([FromQuery] string skuId)
    {
        Logger.LogInformation("Getting image details for product SKU: {SkuId}", skuId);

        try
        {
            if (string.IsNullOrWhiteSpace(skuId))
            {
                Logger.LogWarning("GetProductImagesDetails: SkuId is required");
                return BadRequest("SkuId is required");
            }

            var imageIds = await productImageService.GetImageIdsForSkuId(skuId);
            if (imageIds == null || !imageIds.Any())
            {
                Logger.LogInformation("GetProductImagesDetails: No images found for SKU {SkuId}", skuId);
                return NotFound($"No images found for product {skuId}");
            }

            var imageResponses = new List<ProductImageResponse>();

            foreach (var imageId in imageIds)
            {
                imageResponses.Add(new ProductImageResponse
                {
                    ImageId = imageId,
                    BlobName = imageId,  // You may want to fetch actual blob name from repository
                    IsPrimary = imageId == await productImageService.GetPrimaryImageIdForSkuId(skuId),
                    ContentType = "image/jpeg", // Default; consider fetching from repository
                    DownloadUrl = Url.Action(nameof(DownloadFileByImageId), "FileUpload", new { skuId, imageId }, Request.Scheme)
                });
            }

            return Ok(imageResponses);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error retrieving image details for product SKU: {SkuId}", skuId);
            return BadRequest(new { error = "Failed to retrieve image details", details = ex.Message });
        }
    }

    /// <summary>
    /// Retrieves the primary image identifier associated with the specified SKU identifier.
    /// </summary>
    /// <param name="skuId">The unique identifier of the SKU for which to retrieve the primary image ID. Cannot be null or empty.</param>
    /// <returns>An HTTP 200 response containing the primary image ID if found; otherwise, an HTTP 404 response if no primary
    /// image exists for the specified SKU, or an HTTP 400 response if the SKU identifier is missing.</returns>
    [HttpGet("getPrimaryImageIdForSkuId")]
    [AllowAnonymous]
    public async Task<IActionResult> GetPrimaryImageIdForSkuId([FromQuery] string skuId)
    {
        if (string.IsNullOrEmpty(skuId))
            return BadRequest("SkuId is required.");
        var imageId = await productImageService.GetPrimaryImageIdForSkuId(skuId);
        if (imageId == null)
            return NotFound("No primary image found.");
        return Ok(imageId);
    }

    /// <summary>
    /// Add an additional image to an existing product.
    /// </summary>
    /// <remarks>
    /// Endpoint: POST <c>/api/fileupload/addproductimage</c>
    /// Authorization: Required ([Authorize])
    /// Request body: Multipart form data with File and SkuId.
    /// Success: Returns 201 Created with image metadata (ImageId, BlobName, IsPrimary, ContentType).
    /// Failure: Returns 400 Bad Request for missing/invalid file, 404 Not Found if product doesn't exist, or on exception.
    /// </remarks>
    /// <param name="request">The request payload containing product SKU and image file.</param>
    /// <param name="isPrimaryImage">Indicates whether this image should replace the current primary image (default: false).</param>
    /// <returns>Image metadata including ImageId and BlobName, or an error result.</returns>
    [HttpPost("addproductimage")]
    [Consumes("multipart/form-data")]
    [Authorize]
    public async Task<IActionResult> AddProductImage([FromForm] FileUploadRequest request, [FromQuery] bool isPrimaryImage = false)
    {
        Logger.LogInformation("Adding image for product SKU: {SkuId}", request.SkuId);

        try
        {
            // Validate request
            if (string.IsNullOrWhiteSpace(request.SkuId))
            {
                Logger.LogWarning("AddProductImage: SKU is required");
                return BadRequest("SKU is required");
            }

            if (request.File == null || request.File.Length == 0)
            {
                Logger.LogWarning("AddProductImage: No file provided for SKU {SkuId}", request.SkuId);
                return BadRequest("No file uploaded");
            }

            // Verify product exists
            var product = await productService.GetProductBySku(request.SkuId);
            if (product == null)
            {
                Logger.LogWarning("AddProductImage: Product with SKU {SkuId} not found", request.SkuId);
                return NotFound($"Product with SKU {request.SkuId} not found");
            }

            // Validate file
            const long maxFileSize = 10 * 1024 * 1024; // 10 MB
            var allowedContentTypes = new[] { "image/jpeg", "image/png", "image/webp", "image/gif" };

            if (request.File.Length > maxFileSize)
            {
                Logger.LogWarning("AddProductImage: File size exceeds limit for SKU {SkuId}", request.SkuId);
                return BadRequest($"File size exceeds maximum allowed size of {maxFileSize / (1024 * 1024)} MB");
            }

            if (!allowedContentTypes.Contains(request.File.ContentType?.ToLowerInvariant() ?? string.Empty))
            {
                Logger.LogWarning("AddProductImage: Invalid content type {ContentType} for SKU {SkuId}", request.File.ContentType, request.SkuId);
                return BadRequest($"Invalid file type. Allowed types: {string.Join(", ", allowedContentTypes)}");
            }

            // Prepare file for upload
            var extension = Path.GetExtension(request.File.FileName).ToLowerInvariant();
            var fileUpload = new FileUpload
            {
                SkuId = request.SkuId,
                ImageId = Guid.NewGuid().ToString(),
                ContentType = request.File.ContentType ?? "image/jpeg",
                Content = request.File.OpenReadStream(),
                Extension = extension
            };

            // Upload image
            var blobName = await productImageService.UploadAsync(fileUpload, isPrimaryImage);

            Logger.LogInformation("Image added successfully for SKU {SkuId}. ImageId: {ImageId}", request.SkuId, fileUpload.ImageId);

            return CreatedAtAction(
                nameof(DownloadFileByImageId),
                new { skuId = request.SkuId, imageId = fileUpload.ImageId },
                new
                {
                    ImageId = fileUpload.ImageId,
                    BlobName = blobName,
                    IsPrimary = isPrimaryImage,
                    ContentType = request.File.ContentType
                });
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error adding image for product SKU: {SkuId}", request.SkuId);
            return BadRequest(new { error = "Failed to add image", details = ex.Message });
        }
    }

    /// <summary>
    /// Delete an image from a product.
    /// </summary>
    /// <remarks>
    /// Endpoint: DELETE <c>/api/fileupload/deleteproductimage</c>?skuId={skuId}&amp;imageId={imageId}
    /// Authorization: Required ([Authorize])
    /// Query parameters: skuId and imageId (both required).
    /// Success: Returns 204 No Content if image is deleted successfully.
    /// Failure: Returns 400 Bad Request for missing parameters, 404 Not Found if image doesn't exist, or on exception.
    /// </remarks>
    /// <param name="skuId">The SKU of the product.</param>
    /// <param name="imageId">The unique identifier of the image to delete.</param>
    /// <returns>204 No Content on success, or an error result.</returns>
    [HttpDelete("deleteproductimage")]
    [Authorize]
    public async Task<IActionResult> DeleteProductImage([FromQuery] string skuId, [FromQuery] string imageId)
    {
        Logger.LogInformation("Deleting image {ImageId} from product SKU: {SkuId}", imageId, skuId);

        try
        {
            // Validate parameters
            if (string.IsNullOrWhiteSpace(skuId))
            {
                Logger.LogWarning("DeleteProductImage: SKU is required");
                return BadRequest("SKU is required");
            }

            if (string.IsNullOrWhiteSpace(imageId))
            {
                Logger.LogWarning("DeleteProductImage: ImageId is required");
                return BadRequest("ImageId is required");
            }

            // Delete the image
            var deleted = await productImageService.DeleteProductImageAsync(skuId, imageId);

            if (!deleted)
            {
                Logger.LogWarning("DeleteProductImage: Image {ImageId} not found for SKU {SkuId}", imageId, skuId);
                return NotFound($"Image {imageId} not found for product {skuId}");
            }

            Logger.LogInformation("Image {ImageId} deleted successfully from product SKU: {SkuId}", imageId, skuId);
            return NoContent();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Error deleting image {ImageId} from product SKU: {SkuId}", imageId, skuId);
            return BadRequest(new { error = "Failed to delete image", details = ex.Message });
        }
    }

}
