namespace BP.Domain.Repository;

public interface IGenericRepository<T> where T : class
{
    T Add(T entity);
    T Update(T entity);
    void Delete(T entity);
    T? GetById(string id);
    IEnumerable<T> GetAll();
}
