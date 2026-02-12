using BP.Api.Contracts;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FeatureController : BaseController
{
    private readonly IFeatureService _featureService;
    private readonly IMetaDataService _metaDataService;

    public FeatureController(IFeatureService featureService, IMetaDataService metaDataService, ILogger<FeatureController> logger) : base(logger)
    {
        _featureService = featureService;
        _metaDataService = metaDataService;
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

    [HttpPut("update")]
    [Authorize]
    public async Task<IActionResult> UpdateFeature([FromBody] AddFeatureRequest feature)
    {
        try
        {
            if (feature == null || string.IsNullOrWhiteSpace(feature.FeatureId))
                return BadRequest("Invalid feature");

            var entity = new MetaDataEntity
            {
                PartitionKey = "Feature",
                RowKey = feature.FeatureId,
                Title = feature.FeatureName ?? string.Empty,
                Notes = feature.symbolic
            };

            await _featureService.Update(feature.FeatureId, feature.FeatureName!, feature.symbolic);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e);
        }
    }
}
