using BP.PriceTracker.Services.Interfaces;
using BP.PriceTracker.Services.Options;
using BP.PriceTracker.Services.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
namespace BP.PriceTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            
#if DEBUG
            builder.Logging.AddDebug();
#endif

            var configBuilder = new ConfigurationBuilder();
#if DEBUG
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.development.json").GetAwaiter().GetResult();
            builder.Configuration.AddJsonStream(stream);

#else
            using var stream = FileSystem.OpenAppPackageFileAsync("appsettings.Production.json").GetAwaiter().GetResult();
            builder.Configuration.AddJsonStream(stream);
#endif
            //var configuration = configBuilder.Build();
            configBuilder.AddEnvironmentVariables();

            builder.Services.Configure<ApiSettings>(builder.Configuration.GetSection(nameof(ApiSettings)));

            builder.Services.AddTransient<ViewModels.LoginViewModel>();
            builder.Services.AddTransientWithShellRoute<Views.HomeView,ViewModels.HomeViewModel>(Constants.Routes.HomeView);


            builder.Services.AddTransient<IUserService,UserService>();
            builder.Services.AddTransient<IApiService,ApiService>();

            return builder.Build();
        }
    }
}
