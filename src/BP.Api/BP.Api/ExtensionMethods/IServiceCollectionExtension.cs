using Azure.Data.Tables;
using Azure.Storage.Blobs;
using BP.Application.Interfaces.Services;
using BP.Application.Services;

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
        services.AddTransient<ISkuGeneratorService, SkuGeneratorService>();
        services.AddTransient<IFileUploadService, FileUploadService>();
        services.AddTransient<IProductImageService, ProductImageService>();
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<BP.Domain.Repository.IProductRepository, BP.Infrastructure.Repositories.ProductRepository>();
        services.AddTransient<BP.Domain.Repository.IUserRepository, BP.Infrastructure.Repositories.UserRepository>();
        services.AddTransient<BP.Domain.Repository.IFileUploadRepository, BP.Infrastructure.Repositories.AzureBlobFileRepository>();
        services.AddTransient<BP.Domain.Repository.IProductImageRepository, BP.Infrastructure.Repositories.ProductImageRepository>();
        return services;
    }

    public static IServiceCollection AddAzureTableServices(this IServiceCollection services)
    {
        services.AddKeyedSingleton<TableClient>("Product", (sp, key) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("tables");
            var tableName = config["TableNames:Product"] ?? "Products";

            var serviceClient = new TableServiceClient(connectionString);
            var client = serviceClient.GetTableClient(tableName);
            client.CreateIfNotExists();
            return client;
        });

        services.AddKeyedSingleton<TableClient>("ProductImages", (sp, key) =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("tables");
            var tableName = config["TableNames:ProductImages"] ?? "ProductsImages";

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

    public static IServiceCollection AddAzureBlobServices(this IServiceCollection services)
    {
        services.AddSingleton<BlobContainerClient>(sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("blobs");
            var containerName = config["BlobContainerNames:Images"] ?? "images";
            var client = new BlobContainerClient(connectionString, containerName);
            client.CreateIfNotExists();
            return client;
        });
        return services;
    }
}