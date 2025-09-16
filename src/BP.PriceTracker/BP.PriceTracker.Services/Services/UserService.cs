using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.PriceTracker.Services.Services;

public class UserService : IUserService
{
    public UserDto ValidateUser(string authToken)
    {
        throw new NotImplementedException();
    }
}
