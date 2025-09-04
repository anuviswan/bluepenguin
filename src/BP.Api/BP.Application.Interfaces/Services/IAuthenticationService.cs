namespace BP.Application.Interfaces.Services;

public interface IAuthenticationService
{
    string HashPassword(string password);
    Task<bool> Authenticate(string username, string password);
}
