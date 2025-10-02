using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController(IProductService productService, ISkuGeneratorService skuGeneratorService, ILogger<ProductController> logger) : BaseController(logger)
{
    private IProductService ProductService => productService;
    private ISkuGeneratorService SkuGeneratorService => skuGeneratorService;

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

            // use SKU Generator service to create SKU
            var skuCode = await SkuGeneratorService.GetSkuCode(product.Category, product.Material, product.FeatureCodes.ToArray(), product.CollectionCode, product.YearCode);
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

            var response = await ProductService.AddProduct(newProduct);
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
}
