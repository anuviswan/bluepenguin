using Azure.Data.Tables;
using BP.Api.Options;
using Microsoft.Extensions.Options;

namespace BP.Api.ExtensionMethods;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<BP.Application.Interfaces.Services.IProductService, BP.Application.Services.ProductService>();
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
