namespace BP.PriceTracker.Services.ApiTypes;

public record ValidateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
