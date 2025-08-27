using Azure.Data.Tables;

namespace BP.Domain.Repository;

public interface IGenericRepository<T> where T : class, ITableEntity
{
    Task Add(T entity);
    Task Update(T entity);
    Task Delete(T entity);
    Task<T?> GetById(string paritionId, string searchKey);
    Task<IEnumerable<T>> GetAll();
}
