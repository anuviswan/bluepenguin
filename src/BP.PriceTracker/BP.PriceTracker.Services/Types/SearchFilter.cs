namespace BP.PriceTracker.Services.Types;

public record SearchFilter
{
    public IEnumerable<string> Categories { get; set; } = null!;
    public IEnumerable<string> Materials { get; set; } = null!;
    public IEnumerable<string> Features { get; set; } = null!;
    public IEnumerable<string> Collections { get; set; } = null!;
    public IEnumerable<string> YearCodes { get; set; } = null!;


}
