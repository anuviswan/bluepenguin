using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtisanFavController(IArtisanFavService artisanFavService, IProductService productService, IProductImageService productImageService, ILogger<ArtisanFavController> logger) : BaseController(logger)
{
    private IProductService ProductService => productService;
    private IProductImageService ProductImageService => productImageService;

    [HttpGet("getall")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ArtisanFavItemResponse>>> GetAll()
    {
        try
        {
            var artisanFavs = (await artisanFavService.GetAll().ConfigureAwait(false)).ToList();

            var items = new List<ArtisanFavItemResponse>();
            foreach (var sku in artisanFavs)
            {
                var product = await ProductService.GetProductBySku(sku).ConfigureAwait(false);
                if (product == null) continue;

                var discountedPrice = product.DiscountPrice.HasValue &&
                                      (!product.DiscountExpiryDate.HasValue || product.DiscountExpiryDate.Value > DateTimeOffset.UtcNow)
                    ? product.DiscountPrice.Value
                    : product.Price;

                var blobUrl = await ProductImageService.GetPrimaryImageUrlForSkuId(sku).ConfigureAwait(false);

                items.Add(new ArtisanFavItemResponse
                {
                    Skuid = sku,
                    ProductName = product.ProductName,
                    OriginalPrice = product.Price,
                    DiscountedPrice = discountedPrice == product.Price ? 0 : discountedPrice,
                    BlobUrl = blobUrl
                });
            }

            return Ok(items);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to fetch artisan favs.");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] ArtisanFavRequest request)
    {
        try
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Sku))
            {
                return BadRequest("Invalid request");
            }

            await artisanFavService.Add(request.Sku).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to create artisan fav for sku {Sku}", request.Sku);
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("delete/{sku}")]
    [Authorize]
    public async Task<IActionResult> Delete(string sku)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                return BadRequest("Invalid sku");
            }

            await artisanFavService.Delete(sku).ConfigureAwait(false);
            return Ok();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to delete artisan fav for sku {Sku}", sku);
            return BadRequest(e.Message);
        }
    }
}
