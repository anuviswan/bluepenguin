namespace BP.Domain.Entities;

public class Product
{
    public string? ProductName { get; set; }
    public string SKU { get; set; } = null!;
    public decimal Price { get; set; }
    public int Stock { get; set; }
}
