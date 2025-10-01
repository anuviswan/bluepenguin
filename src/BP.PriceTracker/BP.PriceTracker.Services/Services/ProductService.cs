using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Types;
using Microsoft.Extensions.Options;

namespace BP.PriceTracker.Services.Services;

public class ProductService(IApiService apiService, IOptions<ApiSettings> apiOptions) : IProductService
{
    private ApiSettings ApiSettings => apiOptions.Value;
    public async Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var endpoint = ApiSettings.GetCategoriesEndpoint;
        return await GetDataAsync<Category>(endpoint);
    }

    public async Task<IEnumerable<Material>> GetMaterialsAsync()
    {
        var endpoint = ApiSettings.GetMaterialsEndpoint;
        var response = await apiService.GetAsync<IEnumerable<Material>>(endpoint);

        if (response.IsSuccess)
        {
            return response.Data ?? Enumerable.Empty<Material>();
        }
        return Enumerable.Empty<Material>();
    }

    public async Task<IEnumerable<Feature>> GetFeaturesAsync()
    {
        var endpoint = ApiSettings.GetFeaturesEndpoint;
        return await GetDataAsync<Feature>(endpoint); ;
    }

    public async Task<IEnumerable<Collection>> GetCollectionsAsync()
    {
        var endpoint = ApiSettings.GetCollectionsEndpoint;
        return await GetDataAsync<Collection>(endpoint);
    }

    private async Task<IEnumerable<TReturn>> GetDataAsync<TReturn>(string endpoint)
    {
        var response = await apiService.GetAsync<IEnumerable<TReturn>>(endpoint);


        if (response.IsSuccess)
        {
            return response.Data ?? Enumerable.Empty<TReturn>();
        }
        return Enumerable.Empty<TReturn>();
    }
}
