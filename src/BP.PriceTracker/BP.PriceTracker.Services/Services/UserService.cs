using BP.PriceTracker.Services.ApiTypes;
using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Types;
using Microsoft.Extensions.Options;

namespace BP.PriceTracker.Services.Services;

public class UserService : IUserService
{
    private readonly ApiSettings _apiSettings;
    private readonly IApiService _apiService;
    public UserService(IApiService apiService, IOptions<ApiSettings> apiOptions)
    {
        _apiSettings = apiOptions.Value;
        _apiService = apiService;
    }
    public UserDto ValidateUser(string passKey)
    {
        var endpoint = _apiSettings.ValidateUserEndpoint;
        var request = new ValidateUserRequest 
        { 
            UserName = "admin",
            PassKey = passKey 
        };
        var response = _apiService.PostAsync<ValidateUserRequest,ValidateUserResponse>(_apiSettings.ValidateUserEndpoint,request, null).GetAwaiter().GetResult();   
        return new UserDto 
        { 
            IsAuthenticated = response.IsAuthenticated,
            AuthToken = response.AuthToken
        };
    }
}
