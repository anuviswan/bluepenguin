using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MetaDataController : Controller
{
    private readonly IMetaDataService _metaService;

    public MetaDataController(IMetaDataService metaService)
    {
        _metaService = metaService;
    }

    [HttpPost("feature/create")]
    public async Task<IActionResult> CreateFeature([FromBody] MetaDataEntity feature)
    {
        if (feature == null || string.IsNullOrWhiteSpace(feature.PartitionKey) || string.IsNullOrWhiteSpace(feature.RowKey))
            return BadRequest("Invalid feature");

        var res = await _metaService.Add(feature);
        return Ok(res);
    }

    [HttpGet("feature/getall")]
    public async Task<IActionResult> GetAllFeatures()
    {
        var res = await _metaService.GetByPartition("Feature");
        return Ok(res);
    }
}
