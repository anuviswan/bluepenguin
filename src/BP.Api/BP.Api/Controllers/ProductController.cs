using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

/// <summary>
/// Controller for product operations.
/// Base route: <c>api/product</c>.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ISkuGeneratorService skuGeneratorService, ILogger<ProductController> logger) : BaseController(logger), IProductController
{
    private IProductService ProductService => productService;
    private ISkuGeneratorService SkuGeneratorService => skuGeneratorService;

    /// <summary>
    /// Create a new product.
    /// </summary>
    /// <remarks>
    /// Endpoint: POST <c>/api/product/create</c>
    /// Request body: <see cref="CreateProductRequest"/>.
    /// Success: Returns 200 OK with the created product SKU as a string.
    /// Failure: Returns 400 Bad Request for invalid model state or on exception.
    /// </remarks>
    /// <param name="product">The product creation request payload.</param>
    /// <returns>The SKU string of the created product or an error result.</returns>
    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest product)
    {
        Logger.LogInformation("Creating Product");

        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state for Product");
                return BadRequest("Invalid model state");
            }

            // use SKU Generator service to create SKU
            var skuCode = await SkuGeneratorService.GetSkuCode(product.Category, product.Material, product.FeatureCodes.ToArray(), product.CollectionCode, product.YearCode);
            var newProduct = new ProductEntity
            {
                PartitionKey = product.Category,
                RowKey = skuCode,
                ProductName = product.Name,
                Price = product.Price,
                SKU = skuCode,
                Stock = 0,
                MaterialCode = product.Material,
                CollectionCode = product.CollectionCode,
                FeatureCodes = string.Join(',', product.FeatureCodes),
                YearCode = product.YearCode,
            };

            var response = await ProductService.AddProduct(newProduct);
            return Ok(response?.SKU);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

    }

    /// <summary>
    /// Get a product by SKU.
    /// </summary>
    /// <remarks>
    /// Endpoint: GET <c>/api/product/getbysku</c>?sku={sku}
    /// Query parameter: <c>sku</c> (string) - the product SKU to lookup.
    /// Success: Returns 200 OK with the product entity.
    /// Not Found: Returns 404 if no product matches the provided SKU.
    /// Failure: Returns 400 Bad Request for invalid SKU or on exception.
    /// </remarks>
    /// <param name="sku">The SKU of the product to retrieve.</param>
    /// <returns>The matching product or an error result.</returns>
    [HttpGet]
    [Route("getbysku")]
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
            var product = await ProductService.GetProductBySku(sku);
            if (product == null)
            {
                Logger.LogWarning($"Product with SKU {sku} not found");
                return NotFound($"Product with SKU {sku} not found");
            }
            return Ok(product);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /// <summary>
    /// Get all products.
    /// </summary>
    /// <remarks>
    /// Endpoint: GET <c>/api/product/getall</c>
    /// Success: Returns 200 OK with a collection of products.
    /// Failure: Returns 400 Bad Request on exception.
    /// </remarks>
    /// <returns>List of all products or an error result.</returns>
    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAllProducts()
    {
        Logger.LogInformation("Getting all Products");
        try
        {
            var products = await ProductService.GetAllProducts();
            return Ok(products);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    /// <summary>
    /// Search products by a set of filters.
    /// </summary>
    /// <remarks>
    /// Endpoint: POST <c>/api/product/search</c>
    /// Body: Object that lists allowed values for each supported attribute. Example:
    /// {
    ///   "SelectedCategories": ["Rings", "Necklaces"],
    ///   "SelectedMaterials": ["M1", "M2"],
    ///   "SelectedFeatures": ["F1","F2"]
    /// }
    /// Success: 200 OK with matching products.
    /// Failure: 400 Bad Request on invalid input or exception.
    /// </remarks>
    [HttpPost]
    [Route("search")]
    public async Task<IActionResult> SearchProducts([FromBody] SearchProductsRequest filters)
    {
        Logger.LogInformation("Searching products with filters");

        try
        {
            if (filters == null ||
                (filters.SelectedCategories == null || !filters.SelectedCategories.Any()) &&
                (filters.SelectedMaterials == null || !filters.SelectedMaterials.Any()) &&
                (filters.SelectedCollections == null || !filters.SelectedCollections.Any()) &&
                (filters.SelectedFeatures == null || !filters.SelectedFeatures.Any()) &&
                (filters.SelectedYears == null || !filters.SelectedYears.Any()))
            {
                Logger.LogWarning("SearchProducts called with empty filters");
                return BadRequest("No filters provided");
            }

            var results = await ProductService.SearchProductsAsync(filters.SelectedCategories,
                filters.SelectedMaterials,
                filters.SelectedCollections,
                filters.SelectedFeatures,
                filters.SelectedYears);
            return Ok(results);
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
}
