using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureController : BaseController
{
    private readonly IFeatureService _featureService;

    public FeatureController(IFeatureService featureService, ILogger<FeatureController> logger) : base(logger)
    {
        _featureService = featureService;
    }

    [HttpGet]
    [Route("getall")]
    public async Task<IActionResult> GetAll()
    {
        Logger.LogInformation("Get All Features");
        try
        {
            var features = await _featureService.GetAllFeatures();
            var response = features.Select(x => new { Id = x.RowKey, Name = x.Title});
            return Ok(response);
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    [HttpPost("create")]
    [Authorize]
    public async Task<IActionResult> CreateFeature([FromBody] AddFeatureRequest feature)
    {
        try
        {
            if (feature == null || string.IsNullOrWhiteSpace(feature.FeatureId) || string.IsNullOrWhiteSpace(feature.FeatureName))
                return BadRequest("Invalid feature");

            await _featureService.Add(feature.FeatureId, feature.FeatureName, feature.symbolic);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }

    }
}
