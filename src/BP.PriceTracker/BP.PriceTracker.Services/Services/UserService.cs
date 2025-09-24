using BP.PriceTracker.Services.ApiTypes;
using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Types;
using Microsoft.Extensions.Options;

namespace BP.PriceTracker.Services.Services;

public class UserService(IApiService apiService, IOptions<ApiSettings> apiOptions) : IUserService
{
    private readonly ApiSettings _apiSettings = apiOptions.Value;

    public async Task<UserDto> ValidateUser(string passKey)
    {
        var endpoint = _apiSettings.ValidateUserEndpoint;
        var request = new ValidateUserRequest 
        { 
            UserName = "admin",
            PassKey = passKey 
        };
        var response = await apiService.PostAsync<ValidateUserRequest,ValidateUserResponse?>(_apiSettings.ValidateUserEndpoint,request, null);

        if (response.IsSuccess)
        {
            return new UserDto 
            { 
                IsAuthenticated = response.Data?.IsAuthenticated ?? false,
                AuthToken = response.Data?.AuthToken ?? string.Empty
            };
        }
        return new UserDto 
        { 
            IsAuthenticated = false,
            AuthToken = string.Empty
        };
    }
}
