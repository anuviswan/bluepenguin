using BP.PriceTracker.Services.Types;

namespace BP.PriceTracker.Services.Interfaces;

public  interface IUserService
{
    UserDto ValidateUser(string authToken);
}
