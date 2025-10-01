using BP.Api.ExtensionMethods;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialController : BaseController
{
    private readonly IMaterialService _materialService;
    public MaterialController(IMaterialService materialService, ILogger<MaterialController> logger) : base(logger)
    {
        _materialService = materialService;
    }
    [HttpGet]
    [Route("getall")]
    public Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Materials");
        try
        {
            var materials = _materialService.GetAllMaterials().Select(x => new { Key = x.ToString(), Value = x.GetDescription() });
            return Task.FromResult<IActionResult>(Ok(materials));
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(BadRequest(e));
        }
    }
}
