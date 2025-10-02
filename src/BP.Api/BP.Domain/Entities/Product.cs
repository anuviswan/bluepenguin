using Azure;
using Azure.Data.Tables;

namespace BP.Domain.Entities;

public record Product : ITableEntity
{
    public string? ProductName { get; set; }
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
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
