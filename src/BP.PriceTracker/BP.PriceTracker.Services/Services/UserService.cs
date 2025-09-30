using BP.PriceTracker.Services.ApiTypes;
using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Types;
using Microsoft.Extensions.Options;

namespace BP.PriceTracker.Services.Services;

public class UserService(IApiService apiService, IOptions<ApiSettings> apiOptions) : IUserService
{
    private ApiSettings ApiSettings => apiOptions.Value;

    public async Task<UserDto> ValidateUser(string passKey)
    {
        var endpoint = ApiSettings.ValidateUserEndpoint;
        var request = new ValidateUserRequest 
        { 
            UserName = "admin",
            Password = passKey 
        };
        var response = await apiService.PostAsync<ValidateUserRequest,ValidateUserResponse?>(ApiSettings.ValidateUserEndpoint,request, null);

        if (response.IsSuccess)
        {
            return new UserDto 
            { 
                IsAuthenticated = true,
                AuthToken = response.Data?.Token ?? string.Empty
            };
        }
        return new UserDto 
        { 
            IsAuthenticated = false,
            AuthToken = string.Empty
        };
    }
}
