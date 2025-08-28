using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureController:BaseController
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService, ILogger<FeatureController> logger):base(logger)
    {
        _featureService = featureService;
    }

    [HttpGet]
    [Route("getall")]
    public Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Features");
        try
        {
            var features = _featureService.GetAllFeatures();
            return Task.FromResult<IActionResult>(Ok(features));
        }
        catch (Exception e)
        {
            return Task.FromResult<IActionResult>(BadRequest(e));
        }
    }
}
