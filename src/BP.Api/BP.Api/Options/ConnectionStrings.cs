namespace BP.Api.Options;

public record ConnectionStrings
{
    public string Storage { get; init; } = null!;
    public string ProductTableName { get; init; } = "Products";
    public string UserTableName { get; init; } = "Products";
}
