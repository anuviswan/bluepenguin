using Azure;
using Azure.Data.Tables;

namespace BP.Domain.Entities;

public record ProductEntity : ITableEntity
{
    public string? ProductName { get; set; }
    public string SKU { get; set; } = null!;
    public double Price { get; set; }
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
    public string Extension { get; set; } = null!;

}