using Azure.Data.Tables;
using BP.Api.Options;
using Microsoft.Extensions.Options;

namespace BP.Api.ExtensionMethods;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<BP.Application.Interfaces.Services.IAuthenticationService, BP.Application.Services.AuthenticationService>();
        services.AddTransient<BP.Application.Interfaces.Services.IProductService, BP.Application.Services.ProductService>();
        services.AddTransient<BP.Application.Interfaces.Services.ICollectionService, BP.Application.Services.CollectionService>();
        services.AddTransient<BP.Application.Interfaces.Services.ICategoryService, BP.Application.Services.CategoryService>();
        services.AddTransient<BP.Application.Interfaces.Services.IFeatureService, BP.Application.Services.FeatureService>();
        services.AddTransient<BP.Application.Interfaces.Services.IMaterialService, BP.Application.Services.MaterialService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<BP.Domain.Repository.IProductRepository, BP.Infrastructure.Repositories.ProductRepository>();
        return services;
    }

    public static IServiceCollection AddAzureTableServices(this IServiceCollection services)
    {
        services.AddSingleton<TableClient>(sp =>
        {
            var opts = sp.GetRequiredService<IOptions<TableStorageOptions>>().Value;
            var serviceClient = new TableServiceClient(opts.ConnectionString);
            return serviceClient.GetTableClient(opts.ProductTableName);
        });
        return services;
    }
}
