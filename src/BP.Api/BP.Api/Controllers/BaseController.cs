using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

public abstract class BaseController(ILogger logger): ControllerBase
{
    protected ILogger Logger => logger;
}
