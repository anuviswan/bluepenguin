using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Interfaces;

public  interface IUserService
{
    Task<UserDto> ValidateUser(string passKey);
}
