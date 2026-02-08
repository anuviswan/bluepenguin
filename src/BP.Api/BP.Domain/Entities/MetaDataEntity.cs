using Azure;
using Azure.Data.Tables;

namespace BP.Domain.Entities;

public record MetaDataEntity : ITableEntity
{
    public string PartitionKey { get; set; } = null!;
    public string RowKey { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Notes { get; set; }
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
