namespace BP.Application.Interfaces.Services;

public interface IArtisanFavService
{
    Task Add(string sku);
    Task<IEnumerable<string>> GetAll();
    Task<IEnumerable<string>> GetLatest(int count);
    Task Delete(string sku);
}
