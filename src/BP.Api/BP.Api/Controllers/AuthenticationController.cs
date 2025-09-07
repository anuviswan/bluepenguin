
using BP.Application.Interfaces.Services;
using BP.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BP.Api.Controllers;

public partial class AuthenticationController : BaseController
{
    private readonly IAuthenticationService _authenticationService;
    private readonly ITokenService _tokenService;
    private readonly IConfiguration _configuration;
    public AuthenticationController(IAuthenticationService authenticationService,ITokenService tokenService, IConfiguration config, ILogger<AuthenticationController> logger) : base(logger)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _configuration = config;
    }

    [AllowAnonymous]
    [Route("login")]
    [HttpPost]
    public async Task<ActionResult<AuthenticationResponse>> Authenticate(AuthenticationRequest request)
    {
        Logger.LogInformation("Login action requested");

        try
        {
            var response = await _authenticationService.Authenticate(request.Username, request.Password);

            if(response == true)
            {
                Logger.LogWarning($"Authentication Succeeded for {request.Username}");

                var generatedToken = _tokenService.BuildToken(_configuration["JwtOptions:Key"]!.ToString(), _configuration["JwtOptions:Issuer"]!.ToString(), request.Username);
                
                return Ok(new AuthenticationResponse
                {
                    Token = generatedToken,
                    UserId = request.Username,
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
    [AllowAnonymous]
    public ActionResult<string> Hashme(string key)
    {
        return Ok(_authenticationService.HashPassword(key));
    }

}
