using Azure;
using Azure.Data.Tables;

namespace BP.Domain.Entities;

public record SectionProductEntity : ITableEntity
{
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }

    public string Sku
    {
        get => RowKey;
        set => RowKey = value;
    }
}
