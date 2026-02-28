using BP.Application.Interfaces.Services;
using BP.Api.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShowcaseController : BaseController
{
    private readonly IShowcaseService _showcaseService;
    private readonly IProductImageService _productImageService;
    private readonly IProductService _productService;
    private readonly ICollectionService _collectionService;

    public ShowcaseController(IShowcaseService showcaseService, IProductImageService productImageService, IProductService productService, ICollectionService collectionService, ILogger<ShowcaseController> logger) : base(logger)
    {
        _showcaseService = showcaseService;
        _productImageService = productImageService;
        _productService = productService;
        _collectionService = collectionService;
    }

    /// <summary>
    /// Gets the top categories for the showcase. Returns category code, name and the primary image URL for the latest SKU in the category.
    /// </summary>
    /// <param name="count">Number of categories to return (default is 4).</param>
    /// <returns>Collection of top categories with primary image URL.</returns>
    [HttpGet]
    [Route("GetTopCategories")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ShowcaseTopCategoryResponse>>> GetTopCategories(int count = 4)
    {
        Logger.LogInformation("Get top categories with count {Count}", count);

        try
        {
            var categories = (await _showcaseService.GetTopCategories(count).ConfigureAwait(false)).ToList();

            var results = new List<ShowcaseTopCategoryResponse>();
            foreach (var c in categories)
            {
                string? blobUrl = null;
                if (!string.IsNullOrWhiteSpace(c.LatestSkuId))
                {
                    blobUrl = await _productImageService.GetPrimaryImageUrlForSkuId(c.LatestSkuId).ConfigureAwait(false);
                }

                results.Add(new ShowcaseTopCategoryResponse
                {
                    CategoryCode = c.CategoryCode,
                    CategoryName = c.CategoryName,
                    BlobUrl = blobUrl
                });
            }

            return Ok(results);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get top categories");
            return BadRequest(e);
        }
    }

    /// <summary>
    /// Gets the top discounted products for the showcase. Returns SKU, product name, original price, effective discounted price and primary image URL.
    /// </summary>
    /// <param name="count">Number of discounted products to return (default is 4).</param>
    /// <returns>Collection of top discounted products with pricing and image URL.</returns>
    [HttpGet]
    [Route("GetTopDiscounts")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ShowcaseTopDiscountResponse>>> GetTopDiscounts(int count = 4)
    {
        Logger.LogInformation("Get top discounts with count {Count}", count);

        try
        {
            var discounts = (await _showcaseService.GetTopDiscounts(count).ConfigureAwait(false)).ToList();

            var results = new List<ShowcaseTopDiscountResponse>();
            foreach (var d in discounts)
            {
                if (string.IsNullOrWhiteSpace(d.SkuId)) continue;

                var product = await _productService.GetProductBySku(d.SkuId).ConfigureAwait(false);
                if (product == null) continue;

                var effectiveDiscountedPrice = product.DiscountPrice.HasValue &&
                                               (!product.DiscountExpiryDate.HasValue || product.DiscountExpiryDate.Value > DateTimeOffset.UtcNow)
                    ? product.DiscountPrice.Value
                    : product.Price;

                var blobUrl = await _productImageService.GetPrimaryImageUrlForSkuId(d.SkuId).ConfigureAwait(false);

                results.Add(new ShowcaseTopDiscountResponse
                {
                    Skuid = d.SkuId,
                    ProductName = product.ProductName,
                    Price = product.Price,
                    DiscountedPrice = effectiveDiscountedPrice,
                    BlobUrl = blobUrl
                });
            }

            return Ok(results);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get top discounts");
            return BadRequest(e);
        }
    }

    /// <summary>
    /// Gets the top collections for the showcase. Returns collection code, name, product count and the primary image URL of the latest product in the collection.
    /// </summary>
    /// <param name="count">Number of collections to return (default is 4).</param>
    /// <returns>Collection of top collections with names, product count and latest product image URL.</returns>
    [HttpGet]
    [Route("GetTopCollections")]
    [AllowAnonymous]
    public async Task<ActionResult<IEnumerable<ShowcaseTopCollectionResponse>>> GetTopCollections(int count = 4)
    {
        Logger.LogInformation("Get top collections with count {Count}", count);

        try
        {
            var collections = (await _showcaseService.GetTopCollections(count).ConfigureAwait(false)).ToList();
            var allCollections = (await _collectionService.GetAllCollections().ConfigureAwait(false)).ToDictionary(c => c.RowKey, c => c.Title, StringComparer.OrdinalIgnoreCase);

            var results = new List<ShowcaseTopCollectionResponse>();
            foreach (var col in collections)
            {
                string? blobUrl = null;
                if (!string.IsNullOrWhiteSpace(col.LatestSkuId))
                {
                    blobUrl = await _productImageService.GetPrimaryImageUrlForSkuId(col.LatestSkuId).ConfigureAwait(false);
                }

                var collectionName = allCollections.TryGetValue(col.CollectionCode, out var title) ? title : col.CollectionCode;

                results.Add(new ShowcaseTopCollectionResponse
                {
                    CollectionCode = col.CollectionCode,
                    CollectionName = collectionName,
                    ProductCount = col.ProductCount,
                    BlobUrl = blobUrl
                });
            }

            return Ok(results);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to get top collections");
            return BadRequest(e);
        }
    }

}
