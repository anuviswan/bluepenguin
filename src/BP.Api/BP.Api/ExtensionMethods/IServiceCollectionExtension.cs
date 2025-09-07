using Azure.Data.Tables;
using BP.Api.Options;
using BP.Application.Interfaces.Services;
using BP.Application.Services;
using Microsoft.Extensions.Options;

namespace BP.Api.ExtensionMethods;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddTransient<ITokenService, TokenService>();
        services.AddTransient<IAuthenticationService, AuthenticationService>();
        services.AddTransient<IProductService, ProductService>();
        services.AddTransient<ICollectionService, CollectionService>();
        services.AddTransient<ICategoryService, CategoryService>();
        services.AddTransient<IFeatureService, FeatureService>();
        services.AddTransient<IMaterialService, MaterialService>();
        services.AddKeyedTransient<ISeederService, UserTableSeederService>("User");
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<BP.Domain.Repository.IProductRepository, BP.Infrastructure.Repositories.ProductRepository>();
        services.AddTransient<BP.Domain.Repository.IUserRepository, BP.Infrastructure.Repositories.UserRepository>();
        return services;
    }

    public static IServiceCollection AddAzureTableServices(this IServiceCollection services)
    {
        services.AddKeyedSingleton<TableClient>("Product",(sp,key) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("tables");
            var tableName = config["TableNames:Product"] ?? "Products";

            var serviceClient = new TableServiceClient(connectionString);
            var client = serviceClient.GetTableClient(tableName);
            client.CreateIfNotExists();
            return client;
        });

        services.AddKeyedSingleton<TableClient>("User", (sp, key) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("tables");
            var tableName = config["TableNames:User"] ?? "Users";

            var serviceClient = new TableServiceClient(connectionString);
            var client = serviceClient.GetTableClient(tableName);
            client.CreateIfNotExists();
            return client;
        });
        return services;
    }
}
