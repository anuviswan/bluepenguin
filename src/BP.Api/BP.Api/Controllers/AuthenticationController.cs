
using BP.Api.Contracts;
using BP.Api.Options;
using BP.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;

namespace BP.Api.Controllers;


public class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly JwtOptions _jwtOptions;
    public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService,  IOptions<JwtOptions> jwtOptions, ILogger<AuthenticationController> logger) : base(logger)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _jwtOptions = jwtOptions.Value;
    }

    [AllowAnonymous]
    [EnableRateLimiting("login")]
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticationRequest request)
    {
        Logger.LogInformation("Login action requested");

        try
        {
            var response = await _authenticationService.Authenticate(request.Username, request.Password);


            if (response == true)
            {
                Logger.LogWarning($"Authentication Succeeded for {request.Username}");

                var generatedToken = _tokenService.BuildToken(_jwtOptions.Key,_jwtOptions.Issuer,_jwtOptions.Audience, request.Username);

                return Ok(new AuthenticationResponse
                {
                    Token = generatedToken,
                    UserId = request.Username,
                    Expiration = DateTime.UtcNow.AddDays(1)
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


    [HttpGet]
    [Route("hashme/{key}")]
    [Authorize]
    public ActionResult<string> Hashme(string key)
    {
        return Ok(_authenticationService.HashPassword(key));
    }

}
