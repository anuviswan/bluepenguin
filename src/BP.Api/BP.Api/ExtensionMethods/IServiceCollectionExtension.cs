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
}
