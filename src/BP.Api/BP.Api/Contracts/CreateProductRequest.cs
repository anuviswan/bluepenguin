namespace BP.Api.Contracts;

public record CreateProductRequest
{
    public string Name { get; init; }
    public decimal Price { get; init; }
}
