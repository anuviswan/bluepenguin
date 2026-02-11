namespace BP.Application.Interfaces.Services;

public interface ITokenService
{
    string BuildToken(string key, string issuer,List<string> audience, string userName);
}
