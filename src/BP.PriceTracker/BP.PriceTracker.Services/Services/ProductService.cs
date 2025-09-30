using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Types;
using Microsoft.Extensions.Options;

namespace BP.PriceTracker.Services.Services;

public class ProductService(IApiService apiService, IOptions<ApiSettings> apiOptions) : IProductService
{
    private ApiSettings ApiSettings => apiOptions.Value;
    public Task<IEnumerable<Category>> GetCategoriesAsync()
    {
        var endpoint = ApiSettings.GetCategoriesEndpoint;
        throw new NotImplementedException();
    }
}
