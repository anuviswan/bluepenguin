namespace BP.Application.Interfaces.Services;

public interface IFeaturedCategoryService
{
    Task Add(string code);
    Task Delete(string code);
    Task<IEnumerable<string>> GetAll();
}
