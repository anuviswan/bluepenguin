using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductController:BaseController
{
    private readonly IProductService _productService;
    public ProductController(IProductService productService, ILogger<ProductController> logger):base(logger)
    {
        _productService = productService;
    }

    [HttpPost]
    [Route("create")]
    public IActionResult CreateProduct(Product product)
    {
        Logger.LogInformation("Creating Product");

        try
        {

        }
        catch (Exception)
        {

            throw;
        }
        _productService.AddProduct(product);

        return Ok("ProductController is working!");
    }
}
