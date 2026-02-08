namespace BP.Api.Contracts;

public record CreateProductRequest
{
    public string ProductName { get; init; } = null!;
    public string Description { get; init; } = string.Empty;
    public IEnumerable<string> Specifications { get; init; } = [];

    public IEnumerable<string> ProductCareInstructions { get; init; } = [];
    public double Price { get; init; }

    public string CategoryCode { get; init; } = null!;

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