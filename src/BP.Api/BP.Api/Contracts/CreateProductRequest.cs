namespace BP.Api.Contracts;

public record CreateProductRequest
{
    public string Name { get; init; } = null!;
    public decimal Price { get; init; }

    public string Category { get; init; } = null!;

    public string Material { get; init; } = null!;
    public IEnumerable<string> FeatureCodes { get; init; } = [];

    public string CollectionCode { get; init; } = null!;

    public int YearCode { get; init; }

    public int SequenceCode { get; init; }

}

public record CreateProductResponse
{
    public string SKUCode { get; init; } = null!;
}