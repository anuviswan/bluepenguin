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
        var response = await apiService.GetAsync<IEnumerable<Category>>(endpoint);

        if (response.IsSuccess)
        {
            return response.Data ?? Enumerable.Empty<Category>();
        }
        return Enumerable.Empty<Category>();
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
        var response = await apiService.GetAsync<IEnumerable<Feature>>(endpoint);

        if (response.IsSuccess)
        {
            return response.Data ?? Enumerable.Empty<Feature>();
        }
        return Enumerable.Empty<Feature>();
    }
}
