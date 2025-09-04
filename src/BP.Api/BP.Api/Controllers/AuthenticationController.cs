
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

public partial class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    public AuthenticationController(IAuthenticationService authenticationService, ILogger<AuthenticationController> logger) : base(logger)
    {
        _authenticationService = authenticationService;
    }


    [HttpPost("login")]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
    {
        Logger.LogInformation("Login action requested");

        try
        {
            var response = _authenticationService.Authenticate(request.Username, request.Password);

            if(response?.IsAuthenticated == true)
            {
                Logger.LogWarning($"Authentication Succeeded for {response.UserName}");
                return Ok(new AuthenticationResponse
                {
                    Token = response.Token!,
                    UserId = response.UserName,
                    Expiration = response.Expiration!.Value
                });
            }

            Logger.LogWarning($"Authentication Failed for {request.Username}");
            return Unauthorized("Invalid credentials");


        }
        catch (Exception)
        {
            return Unauthorized("Invalid credentials");
        }
    }
}
