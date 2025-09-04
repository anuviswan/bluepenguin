using BP.Application.Interfaces.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.Application.Interfaces.Services;

public interface IAuthenticationService
{
    AuthenticationValidation Authenticate(string username, string password);
}
