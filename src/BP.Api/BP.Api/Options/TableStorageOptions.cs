namespace BP.Api.Options;

public record TableStorageOptions
{
    public string ConnectionString { get; init; } = null!;
    public string ProductTableName { get; init; } = "Products";
}
