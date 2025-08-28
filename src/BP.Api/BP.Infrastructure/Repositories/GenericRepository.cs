using Azure.Data.Tables;
using BP.Domain.Repository;

namespace BP.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class, ITableEntity
{
    private TableClient _tableClient;
    public GenericRepository(TableClient tableClient)
    {
        _tableClient = tableClient;
        if(TableClient.CreateIfNotExists() == null)
        {
            throw new Exception($"Table {tableClient.Name} could not be created.");
        }
    }

    public TableClient TableClient => _tableClient;
    public async Task Add(T entity)
    {
        await TableClient.AddEntityAsync(entity);
    }

    public async Task Delete(T entity)
    {
        await TableClient.DeleteEntityAsync(entity.PartitionKey, entity.RowKey);
    }

    public async Task<IEnumerable<T>> GetAll()
    {
        var result = new List<T>();
        await foreach (var entity in TableClient.QueryAsync<T>())
        {
            result.Add(entity);
        }

        return result;
    }

    public async Task<T?> GetById(string paritionId, string searchKey)
    {
        return await TableClient.GetEntityAsync<T>(partitionKey:paritionId, rowKey:searchKey);
    }

    public async Task Update(T entity)
    {
        await TableClient.UpdateEntityAsync(entity,entity.ETag,TableUpdateMode.Replace);
    }
}
