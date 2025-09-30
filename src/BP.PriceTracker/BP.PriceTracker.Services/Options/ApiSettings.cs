using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BP.PriceTracker.Services.Options;

public record ApiSettings
{
    public string BaseUrl { get; init; } = string.Empty;
    public string ValidateUserEndpoint { get; init; } = string.Empty;
    public string GetCategoriesEndpoint { get; init; } = string.Empty;
    public string GetMaterialsEndpoint { get; init; } = string.Empty;
}
