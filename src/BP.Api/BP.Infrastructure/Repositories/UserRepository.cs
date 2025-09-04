using Azure.Data.Tables;
using BP.Domain.Entities;
using BP.Domain.Repository;

namespace BP.Infrastructure.Repositories;

public class UserRepository : GenericRepository<UserEntity>, IUserRepository
{
    public UserRepository(TableClient tableClient) : base(tableClient)
    {
    }

    public async Task<UserEntity?> GetUser(string username)
    {
       return await GetById("Admin",username!);
    }
}
