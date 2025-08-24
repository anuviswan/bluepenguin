using BP.Domain.Repository;

namespace BP.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    public T Add(T entity)
    {
        throw new NotImplementedException();
    }

    public void Delete(T entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> GetAll()
    {
        throw new NotImplementedException();
    }

    public T? GetById(string id)
    {
        throw new NotImplementedException();
    }

    public T Update(T entity)
    {
        throw new NotImplementedException();
    }
}
