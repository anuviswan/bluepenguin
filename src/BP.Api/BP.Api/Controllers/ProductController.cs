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

            // use SKU Generator service to create SKU
            var skuCode = skuId ?? await SkuGeneratorService.GetSkuCode(product.CategoryCode, product.Material, product.FeatureCodes.ToArray(), product.CollectionCode, product.YearCode).ConfigureAwait(false);

            if(await skuGeneratorService.CheckIfSkuExists(skuCode))
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
            var product = await ProductService.GetProductBySku(sku);
            if (product == null)
            {
                Logger.LogWarning($"Product with SKU {sku} not found");
                return NotFound($"Product with SKU {sku} not found");
            }

            var response = new 
            {
                product.SKU,
                CategoryCode = product.PartitionKey,
                product.ProductName,
                product.ProductDescription,
                product.ProductCareInstructions,
                product.Specifications,
                product.Price,
                product.Stock,
                product.MaterialCode,
                product.CollectionCode,
                FeatureCodes = product.FeatureCodes.Split(','),
                product.YearCode
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
    /// <remarks>
    /// Endpoint: GET <c>/api/product/getall</c>
    /// Success: Returns 200 OK with a collection of products.
    /// Failure: Returns 400 Bad Request on exception.
    /// </remarks>
    /// <returns>List of all products or an error result.</returns>
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

            var paged = products
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new
                {
                    p.SKU,
                    CategoryCode = p.PartitionKey,
                    p.ProductName,
                    p.ProductDescription,
                    p.ProductCareInstructions,
                    p.Specifications,
                    p.Price,
                    p.Stock,
                    p.MaterialCode,
                    p.CollectionCode,
                    p.FeatureCodes,
                    p.YearCode
                });

            return Ok(new { totalCount, page, pageSize, items = paged });
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
                        filters.SelectedYears)).ToList();

                    var totalCount = results.Count;
                    if (page <= 0) page = 1;
                    if (pageSize <= 0) pageSize = 50;

                    var paged = results.Skip((page - 1) * pageSize).Take(pageSize);
                    return Ok(new { totalCount, page, pageSize, items = paged });
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

            /// <summary>
            /// Update an existing product.
            /// </summary>
            /// <remarks>
            /// Endpoint: PUT <c>/api/product/update</c>
            /// Authorization: Required ([Authorize])
            /// Request body: <see cref="UpdateProductRequest"/>.
            /// Success: Returns 200 OK with the updated product SKU.
            /// Failure: Returns 400 Bad Request if SKU already exists or on exception.
            /// </remarks>
            /// <param name="request">The product update request payload.</param>
            /// <returns>The SKU of the updated product or an error result.</returns>
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

                    // Get the existing product
                    var existingProduct = await ProductService.GetProductBySku(request.SKU);
                    if (existingProduct == null)
                    {
                        Logger.LogWarning($"Product with SKU {request.SKU} not found");
                        return NotFound($"Product with SKU {request.SKU} not found");
                    }

                    // Update only the provided fields
                    if (!string.IsNullOrWhiteSpace(request.ProductName))
                        existingProduct.ProductName = request.ProductName;

                    if (!string.IsNullOrWhiteSpace(request.Description))
                        existingProduct.ProductDescription = request.Description;

                    if (request.Price.HasValue)
                        existingProduct.Price = request.Price.Value;

                    if (request.Stock.HasValue)
                        existingProduct.Stock = request.Stock.Value;

                    if (request.Specifications != null && request.Specifications.Any())
                        existingProduct.Specifications = request.Specifications;

                    if (request.ProductCareInstructions != null && request.ProductCareInstructions.Any())
                        existingProduct.ProductCareInstructions = request.ProductCareInstructions;

                    // Call service to update (which validates SKU)
                    var updatedProduct = await ProductService.UpdateProduct(existingProduct);

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
        }
