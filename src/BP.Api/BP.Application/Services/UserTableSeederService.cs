using Azure.Data.Tables;
using BP.Application.Interfaces.Options;
using BP.Application.Interfaces.Services;
using BP.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BP.Application.Services;

public class UserTableSeederService([FromKeyedServices("User")] TableClient userTable, IAuthenticationService authenticationService, IOptions<UserTableSeedingOptions> seedingInfo) : ISeederService
{
    public async Task SeedAsync()
    {
        var seedingOptions = seedingInfo.Value;
        // Make sure the table exists
        await userTable.CreateIfNotExistsAsync();

        // Check if already seeded
        var existing = userTable.Query<UserEntity>(u => u.PartitionKey == "Admin").Any();
        if (existing) return;

        // Seed default users
        foreach (var user in seedingOptions.Users)
        {
            var userEntity = new UserEntity
            {
                PartitionKey = "Admin",
                RowKey = user.userName,
                Password = authenticationService.HashPassword(user.password)
            };
            await userTable.AddEntityAsync(userEntity);
        }
    }
}
