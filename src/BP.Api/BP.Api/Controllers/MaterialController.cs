using BP.Api.ExtensionMethods;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialController : BaseController
{
    private readonly IMaterialService _materialService;
    private readonly IProductService _productService;

    public MaterialController(IMaterialService materialService, IProductService productService, ILogger<MaterialController> logger) : base(logger)
    {
        _materialService = materialService;
        _productService = productService;
    }

    [HttpGet]
    [Route("getall")]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Materials");
        try
        {
            var materials = _materialService.GetAllMaterials().ToList();
            var products = (await _productService.GetAllProducts().ConfigureAwait(false)).ToList();

            var response = materials.Select(x =>
            {
                var materialId = x.ToString();
                var count = products.Count(p =>
                    !string.IsNullOrWhiteSpace(p.MaterialCode) &&
                    p.MaterialCode.Equals(materialId, StringComparison.OrdinalIgnoreCase));

                return new { Id = x.ToString(), Name = x.GetDescription(), ItemCount = count };
            });

            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
