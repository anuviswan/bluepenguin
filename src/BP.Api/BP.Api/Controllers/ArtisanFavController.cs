using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Application.Interfaces.Options;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtisanFavController : BaseController
{
    private readonly IArtisanFavService artisanFavService;
    private readonly IProductService productService;
    private readonly IProductImageService productImageService;
    private readonly IOptions<ArtisanFavOptions> artisanFavOptions;

    public ArtisanFavController(
        IArtisanFavService artisanFavService,
        IProductService productService,
        IProductImageService productImageService,
        ILogger<ArtisanFavController> logger,
        IOptions<ArtisanFavOptions> artisanFavOptions)
        : base(logger)
    {
        this.artisanFavService = artisanFavService;
        this.productService = productService;
        this.productImageService = productImageService;
        this.artisanFavOptions = artisanFavOptions;
    }

    private IProductService ProductService => productService;
    private IProductImageService ProductImageService => productImageService;
    private int ArtisanFavLimit => artisanFavOptions.Value.Limit;

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

    [HttpGet("latest")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ArtisanFavItemResponse>>> GetLatest([FromQuery] int? count = null)
    {
        try
        {
            int takeCount = count ?? ArtisanFavLimit;
            var latestSkus = (await artisanFavService.GetLatest(takeCount).ConfigureAwait(false)).ToList();
            var items = new List<ArtisanFavItemResponse>();
            foreach (var sku in latestSkus)
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
            Logger.LogError(e, "Failed to fetch latest artisan favs.");
            return BadRequest(e.Message);
        }
    }

    [HttpPost("search-products")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchArtisanFavProducts([FromBody] SearchProductsRequest filters, [FromQuery] int page = 1, [FromQuery] int pageSize = 50, [FromQuery] string? partialProductName = null)
    {
        Logger.LogInformation("Searching artisan fav products with filters");

        try
        {
            if (filters == null ||
                (filters.SelectedCategories == null) &&
                (filters.SelectedMaterials == null) &&
                (filters.SelectedCollections == null) &&
                (filters.SelectedFeatures == null) &&
                (filters.SelectedYears == null))
            {
                Logger.LogWarning("SearchArtisanFavProducts called with empty filters");
                return BadRequest("No filters provided");
            }

            var effectivePartialName = partialProductName ?? filters.PartialProductName;
            var results = (await ProductService.SearchProductsAsync(
                filters.SelectedCategories,
                filters.SelectedMaterials,
                filters.SelectedCollections,
                filters.SelectedFeatures,
                filters.SelectedYears,
                effectivePartialName).ConfigureAwait(false)).ToList();

            var artisanFavSkus = (await artisanFavService.GetAll().ConfigureAwait(false)).ToHashSet(StringComparer.OrdinalIgnoreCase);
            var filteredResults = results.Where(p => artisanFavSkus.Contains(p.SKU)).ToList();

            var totalCount = filteredResults.Count;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 50;

            var pagedResults = filteredResults
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var items = new List<ProductListItemResponse>();
            foreach (var p in pagedResults)
            {
                var primaryImageUrl = await ProductImageService.GetPrimaryImageUrlForSkuId(p.SKU).ConfigureAwait(false);
                items.Add(new ProductListItemResponse
                {
                    Sku = p.SKU,
                    CategoryCode = p.PartitionKey,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductCareInstructions = p.ProductCareInstructions,
                    Specifications = p.Specifications,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    DiscountExpiryDate = p.DiscountExpiryDate,
                    Stock = p.Stock,
                    MaterialCode = p.MaterialCode,
                    CollectionCode = p.CollectionCode,
                    FeatureCodes = p.FeatureCodes?.Split(',') ?? Array.Empty<string>(),
                    YearCode = p.YearCode,
                    PrimaryImageUrl = primaryImageUrl,
                    IsArtisanFav = true
                });
            }

            return Ok(new { totalCount, page, pageSize, items });
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation("SearchArtisanFavProducts request cancelled");
            return BadRequest("Request cancelled");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error searching artisan fav products");
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
