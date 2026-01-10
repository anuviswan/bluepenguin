using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Types;
using Microsoft.Extensions.Options;

namespace BP.PriceTracker.Services.Services;

public class ProductService(IApiService apiService, IOptions<ApiSettings> apiOptions) : IProductService
{
    private ApiSettings ApiSettings => apiOptions.Value;
    private readonly IApiService _apiService = apiService;

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

    public async Task<IEnumerable<ProductDto>> SearchProductsAsync(SearchFilter filters)
    {
        var endpoint = string.IsNullOrWhiteSpace(ApiSettings.SearchProductsEndpoint) ? "/api/Product/search" : ApiSettings.SearchProductsEndpoint;
        var request = new
        {
            SelectedCategories = filters.Categories?.ToArray(),
            SelectedMaterials = filters.Materials?.ToArray(),
            SelectedCollections = filters.Collections?.ToArray(),
            SelectedFeatures = filters.Features?.ToArray(),
            SelectedYears = filters.YearCodes?.ToArray()
        };

        var response = await _apiService.PostAsync<object, IEnumerable<ProductDto>>(endpoint, request);

        if (response.IsSuccess)
        {
            return response.Data ?? Enumerable.Empty<ProductDto>();
        }

        return Enumerable.Empty<ProductDto>();
    }
}
