namespace BP.Application.Interfaces.Options;

public record UserTableSeedingOptions
{
    public IEnumerable<UserSeedValue> Users { get; init; } = [];
};

public record UserSeedValue
{
    public string userName { get; init; } = null!;
    public string password { get; init; } = null!;
};
