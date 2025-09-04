using Azure;
using Azure.Data.Tables;

namespace BP.Domain.Entities;

public record UserEntity : ITableEntity
{
    public string Password { get; set; } = null!;
    public string PartitionKey { get; set; } = null!;  //  Role
    public string RowKey { get; set; } = null!;  // UserName
    public DateTimeOffset? Timestamp { get; set; }
    public ETag ETag { get; set; }
}
