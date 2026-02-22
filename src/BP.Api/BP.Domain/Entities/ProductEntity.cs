using Azure;
using Azure.Data.Tables;
using System.Runtime.Serialization;
using System.Text.Json;

namespace BP.Domain.Entities;

public record ProductEntity : ITableEntity
{
    public string? ProductName { get; set; }
    public string? ProductDescription { get; set; }
    public string ProductCareInstructionsJson
    {
        get => JsonSerializer.Serialize(ProductCareInstructions);
        set => ProductCareInstructions = JsonSerializer.Deserialize<List<string>>(value) ?? [];
    }

    [IgnoreDataMember]
    public IEnumerable<string> ProductCareInstructions { get; set; } = [];
    
    public string SpecificationsJson
    {
        get => JsonSerializer.Serialize(Specifications);
        set => Specifications = JsonSerializer.Deserialize<List<string>>(value) ?? [];
    }

    [IgnoreDataMember]
    public IEnumerable<string> Specifications { get; set; } = [];

    public string SKU { get; set; } = null!;
    public double Price { get; set; }
    public double? DiscountPrice { get; set; }
    public DateTimeOffset? DiscountExpiryDate { get; set; }
    public int Stock { get; set; }
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public string MaterialCode { get; set; } = null!;
    public string CollectionCode { get; set; } = null!;
    public string FeatureCodes { get; set; } = null!;
    public int YearCode { get; set; }

}
