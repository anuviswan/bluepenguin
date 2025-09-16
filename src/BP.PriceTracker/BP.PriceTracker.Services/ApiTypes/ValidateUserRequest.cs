namespace BP.PriceTracker.Services.ApiTypes;

public record ValidateUserRequest
{
    public string UserName { get; set; } = string.Empty;
    public string PassKey { get; set; } = string.Empty;
}
