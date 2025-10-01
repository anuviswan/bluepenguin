namespace BP.Application.Interfaces.Options;

public record UserTableSeedingOptions
{
    public IEnumerable<UserSeedValue> Users { get; init; } = [];
};

public record UserSeedValue
{
    public string userName { get; init; }
    public string password { get; init; }
};
