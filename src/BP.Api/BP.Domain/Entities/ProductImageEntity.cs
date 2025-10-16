using Azure;
using Azure.Data.Tables;

namespace BP.Domain.Entities;

public record ProductImageEntity: ITableEntity
{
    public string PartitionKey { get; set; } = null!; // SKU
    public string RowKey { get; set; } = null!; //image id

    public string BlobName { get; set; } = null!; 
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
    public bool IsPrimary { get; set; }
    public bool IsThumbnail { get; set; }
    public string ContentType { get; set; } = null!;

}