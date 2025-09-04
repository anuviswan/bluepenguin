namespace BP.Application.Interfaces.Services;

public interface IAuthenticationService
{
    Task<bool> Authenticate(string username, string password);
}
