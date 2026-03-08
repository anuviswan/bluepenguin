namespace BP.Api.Contracts;

public class ImageSearchResultResponse
{
    public string SkuId { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
    public double Price { get; set; }
    public double? Discount { get; set; }
    public string? ProductName { get; set; }
}
