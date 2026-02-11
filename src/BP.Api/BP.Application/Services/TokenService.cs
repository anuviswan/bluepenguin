using BP.Application.Interfaces.Services;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BP.Application.Services;

public class TokenService : ITokenService
{
    private TimeSpan ExpiryDuration = new TimeSpan(24, 0, 0);
    public string BuildToken(string key, string issuer, List<string> audience, string userName)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim(ClaimTypes.NameIdentifier,
            Guid.NewGuid().ToString())
         };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        var tokenDescriptor = new JwtSecurityToken(issuer, audience[0], claims,
            expires: DateTime.UtcNow.Add(ExpiryDuration), signingCredentials: credentials);
        tokenDescriptor.Payload["aud"] = audience; // Add the full list of audiences to the token payload
        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
    }
}
