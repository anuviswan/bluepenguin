namespace BP.Api.Controllers;

public partial class AuthenticationController
{
    public record AuthenticationRequest
    {
        public string Username { get; init; } = null!;
        public string Password { get; init; } = null!;
    }
}
