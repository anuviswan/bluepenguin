namespace BP.Application.Interfaces.Services;

public interface IAuthenticationService
{
    bool Authenticate(string username, string password);
}
