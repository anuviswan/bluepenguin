using BP.Domain.Entities;

namespace BP.Domain.Repository;

public interface IUserRepository
{
    Task<UserEntity?> GetUser(string username);
}
