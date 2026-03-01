using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

/// <summary>
/// Controller for product operations.
/// Base route: <c>api/product</c>.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ISkuGeneratorService skuGeneratorService, IProductImageService productImageService, IArtisanFavService artisanFavService, ILogger<ProductController> logger) : BaseController(logger), IProductController
{
    private IProductService ProductService => productService;
    private ISkuGeneratorService SkuGeneratorService => skuGeneratorService;
    private IProductImageService ProductImageService => productImageService;
    private IArtisanFavService ArtisanFavService => artisanFavService;

    /// <summary>
    /// Create a new product.
    /// </summary>
    [HttpPost]
    [Route("create")]
    [Authorize]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest product,
        [FromQuery] string? skuId = null)
    {
        Logger.LogInformation("Creating Product");

        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state for Product");
                return BadRequest("Invalid model state");
            }

            var skuCode = skuId ?? await SkuGeneratorService.GetSkuCode(product.CategoryCode, product.Material, product.FeatureCodes.ToArray(), product.CollectionCode, product.YearCode).ConfigureAwait(false);

            if (await skuGeneratorService.CheckIfSkuExists(skuCode).ConfigureAwait(false))
            {
                return BadRequest($"SKU {skuCode} already exists");
            }

            var newProduct = new ProductEntity
            {
                PartitionKey = product.CategoryCode,
                RowKey = skuCode,
                ProductName = product.ProductName,
                ProductDescription = product.Description,
                ProductCareInstructions = product.ProductCareInstructions,
                Specifications = product.Specifications,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                DiscountExpiryDate = product.DiscountExpiryDate,
                SKU = skuCode,
                Stock = product.Stock,
                MaterialCode = product.Material,
                CollectionCode = product.CollectionCode,
                FeatureCodes = string.Join(',', product.FeatureCodes),
                YearCode = product.YearCode,
            };

            var response = await ProductService.AddProduct(newProduct).ConfigureAwait(false);
            return Ok(response?.SKU);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /// <summary>
    /// Get a product by SKU. Returns product details including all images with SAS URLs and artisan fav status.
    /// </summary>
    [HttpGet]
    [Route("getbysku")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProduct([FromQuery] string sku)
    {
        Logger.LogInformation("Getting Product");
        try
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                Logger.LogError("Invalid SKU");
                return BadRequest("Invalid SKU");
            }

            var product = await ProductService.GetProductBySku(sku).ConfigureAwait(false);
            if (product == null)
            {
                Logger.LogWarning($"Product with SKU {sku} not found");
                return NotFound($"Product with SKU {sku} not found");
            }

            var primaryImageId = await ProductImageService.GetPrimaryImageIdForSkuId(sku).ConfigureAwait(false);
            var imageIds = (await ProductImageService.GetImageIdsForSkuId(sku).ConfigureAwait(false)).ToList();
            
            var images = new List<ProductImageDetailsResponse>();
            foreach (var imageId in imageIds)
            {
                var imageUrl = await ProductImageService.GetImageUrlForImageIdAsync(sku, imageId).ConfigureAwait(false);
                images.Add(new ProductImageDetailsResponse
                {
                    ImageId = imageId,
                    ImageUrl = imageUrl,
                    IsPrimary = imageId.Equals(primaryImageId, StringComparison.OrdinalIgnoreCase)
                });
            }

            var artisanFavs = (await ArtisanFavService.GetAll().ConfigureAwait(false)).ToList();
            var isArtisanFav = artisanFavs.Any(fav => fav.Equals(sku, StringComparison.OrdinalIgnoreCase));

            var response = new ProductResponse
            {
                Sku = product.SKU,
                CategoryCode = product.PartitionKey,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                ProductCareInstructions = product.ProductCareInstructions,
                Specifications = product.Specifications,
                Price = product.Price,
                DiscountPrice = GetEffectiveDiscountPrice(product),
                DiscountExpiryDate = product.DiscountExpiryDate,
                Stock = product.Stock,
                MaterialCode = product.MaterialCode,
                CollectionCode = product.CollectionCode,
                FeatureCodes = product.FeatureCodes.Split(','),
                YearCode = product.YearCode,
                Images = images,
                IsArtisanFav = isArtisanFav
            };
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /// <summary>
    /// Get all products.
    /// </summary>
    [HttpGet]
    [Route("getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllProducts([FromQuery] int page = 1, [FromQuery] int pageSize = 5)
    {
        Logger.LogInformation("Getting all Products");
        try
        {
            var products = (await ProductService.GetAllProducts()).ToList();

            var totalCount = products.Count;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 50;

            var artisanFavs = (await ArtisanFavService.GetAll().ConfigureAwait(false)).ToList();

            var pagedProducts = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var items = new List<ProductListItemResponse>();
            foreach (var p in pagedProducts)
            {
                var primaryImageUrl = await ProductImageService.GetPrimaryImageUrlForSkuId(p.SKU).ConfigureAwait(false);
                var isArtisanFav = artisanFavs.Any(fav => fav.Equals(p.SKU, StringComparison.OrdinalIgnoreCase));

                items.Add(new ProductListItemResponse
                {
                    Sku = p.SKU,
                    CategoryCode = p.PartitionKey,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductCareInstructions = p.ProductCareInstructions,
                    Specifications = p.Specifications,
                    Price = p.Price,
                    DiscountPrice = GetEffectiveDiscountPrice(p),
                    DiscountExpiryDate = p.DiscountExpiryDate,
                    Stock = p.Stock,
                    MaterialCode = p.MaterialCode,
                    CollectionCode = p.CollectionCode,
                    FeatureCodes = p.FeatureCodes.Split(','),
                    YearCode = p.YearCode,
                    PrimaryImageUrl = primaryImageUrl,
                    IsArtisanFav = isArtisanFav
                });
            }

            return Ok(new { totalCount, page, pageSize, items });
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost]
    [Route("search")]
    [AllowAnonymous]
    public async Task<IActionResult> SearchProducts([FromBody] SearchProductsRequest filters, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        Logger.LogInformation("Searching products with filters");

        try
        {
            if (filters == null ||
                (filters.SelectedCategories == null) &&
                (filters.SelectedMaterials == null) &&
                (filters.SelectedCollections == null) &&
                (filters.SelectedFeatures == null) &&
                (filters.SelectedYears == null))
            {
                Logger.LogWarning("SearchProducts called with empty filters");
                return BadRequest("No filters provided");
            }

            var results = (await ProductService.SearchProductsAsync(filters.SelectedCategories,
                filters.SelectedMaterials,
                filters.SelectedCollections,
                filters.SelectedFeatures,
                filters.SelectedYears).ConfigureAwait(false)).ToList();

            var totalCount = results.Count;
            if (page <= 0) page = 1;
            if (pageSize <= 0) pageSize = 50;

            var artisanFavs = (await ArtisanFavService.GetAll().ConfigureAwait(false)).ToList();

            var pagedResults = results
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var items = new List<ProductListItemResponse>();
            foreach (var p in pagedResults)
            {
                var primaryImageUrl = await ProductImageService.GetPrimaryImageUrlForSkuId(p.SKU).ConfigureAwait(false);
                var isArtisanFav = artisanFavs.Any(fav => fav.Equals(p.SKU, StringComparison.OrdinalIgnoreCase));

                items.Add(new ProductListItemResponse
                {
                    Sku = p.SKU,
                    CategoryCode = p.PartitionKey,
                    ProductName = p.ProductName,
                    ProductDescription = p.ProductDescription,
                    ProductCareInstructions = p.ProductCareInstructions,
                    Specifications = p.Specifications,
                    Price = p.Price,
                    DiscountPrice = GetEffectiveDiscountPrice(p),
                    DiscountExpiryDate = p.DiscountExpiryDate,
                    Stock = p.Stock,
                    MaterialCode = p.MaterialCode,
                    CollectionCode = p.CollectionCode,
                    FeatureCodes = p.FeatureCodes.Split(','),
                    YearCode = p.YearCode,
                    PrimaryImageUrl = primaryImageUrl,
                    IsArtisanFav = isArtisanFav
                });
            }

            return Ok(new { totalCount, page, pageSize, items });
        }
        catch (OperationCanceledException)
        {
            Logger.LogInformation("SearchProducts request cancelled");
            return BadRequest("Request cancelled");
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error searching products");
            return BadRequest(e);
        }
    }

    [HttpDelete]
    [Route("delete/{sku}")]
    [Authorize]
    public async Task<IActionResult> DeleteProduct(string sku)
    {
        Logger.LogInformation("Deleting Product");

        try
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                Logger.LogError("Invalid SKU");
                return BadRequest("Invalid SKU");
            }

            await ProductService.DeleteProduct(sku).ConfigureAwait(false);
            return Ok(new { message = "Product deleted successfully", sku });
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error deleting product");
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    [Route("update")]
    [Authorize]
    public async Task<IActionResult> UpdateProduct([FromBody] UpdateProductRequest request)
    {
        Logger.LogInformation("Updating Product");

        try
        {
            if (request == null || string.IsNullOrWhiteSpace(request.SKU))
            {
                Logger.LogError("Invalid update request: SKU is required");
                return BadRequest("Invalid update request: SKU is required");
            }

            var existingProduct = await ProductService.GetProductBySku(request.SKU).ConfigureAwait(false);
            if (existingProduct == null)
            {
                Logger.LogWarning($"Product with SKU {request.SKU} not found");
                return NotFound($"Product with SKU {request.SKU} not found");
            }

            if (!string.IsNullOrWhiteSpace(request.ProductName))
                existingProduct.ProductName = request.ProductName;

            if (!string.IsNullOrWhiteSpace(request.Description))
                existingProduct.ProductDescription = request.Description;

            if (request.Price.HasValue)
                existingProduct.Price = request.Price.Value;

            if (request.DiscountPrice.HasValue)
                existingProduct.DiscountPrice = request.DiscountPrice.Value;

            if (request.DiscountExpiryDate.HasValue)
                existingProduct.DiscountExpiryDate = request.DiscountExpiryDate.Value;

            if (request.Stock.HasValue)
                existingProduct.Stock = request.Stock.Value;

            if (request.Specifications != null && request.Specifications.Any())
                existingProduct.Specifications = request.Specifications;

            if (request.ProductCareInstructions != null && request.ProductCareInstructions.Any())
                existingProduct.ProductCareInstructions = request.ProductCareInstructions;

            var updatedProduct = await ProductService.UpdateProduct(existingProduct).ConfigureAwait(false);

            return Ok(new { message = "Product updated successfully", sku = updatedProduct.SKU });
        }
        catch (InvalidOperationException ex)
        {
            Logger.LogWarning(ex, "Invalid operation while updating product");
            return BadRequest(ex.Message);
        }
        catch (ArgumentException ex)
        {
            Logger.LogWarning(ex, "Invalid argument while updating product");
            return BadRequest(ex.Message);
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Error updating product");
            return BadRequest(e.Message);
        }
    }

    private static double? GetEffectiveDiscountPrice(ProductEntity product)
    {
        if (!product.DiscountPrice.HasValue)
        {
            return null;
        }

        if (product.DiscountExpiryDate.HasValue && product.DiscountExpiryDate.Value < DateTimeOffset.UtcNow)
        {
            return product.Price;
        }

        return product.DiscountPrice.Value;
    }
}
