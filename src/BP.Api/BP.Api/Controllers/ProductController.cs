using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController : BaseController
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService, ILogger<ProductController> logger) : base(logger)
    {
        _productService = productService;
    }

    [HttpPost]
    [Route("create")]
    public async Task<IActionResult> CreateProduct(CreateProductRequest product)
    {
        Logger.LogInformation("Creating Product");

        try
        {
            if (!ModelState.IsValid)
            {
                Logger.LogError("Invalid model state for Product");
                return BadRequest("Invalid model state");
            }

            var collectionCode = await _productService.GetItemCountForCollection(product.CollectionCode, product.YearCode);
            var skuCode = $"{product.Category}{product.Material}-{string.Join('-', product.FeatureCodes)}-{product.CollectionCode}{product.YearCode}{collectionCode + 1}";
            var newProduct = new Product
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
            };

            var response = await _productService.AddProduct(newProduct);
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

    }

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
            var product = await _productService.GetProductBySku(sku);
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

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAllProducts()
    {
        Logger.LogInformation("Getting all Products");
        try
        {
            var products = await _productService.GetAllProducts();
            return Ok(products);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
