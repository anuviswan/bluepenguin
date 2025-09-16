using BP.PriceTracker.Services.Options;
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
            configBuilder.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
#else
            configBuilder.AddJsonFile("appsettings.Production.json", optional: false, reloadOnChange: true);
#endif
            var configuration = configBuilder.Build();
            configBuilder.AddEnvironmentVariables();

            builder.Services.Configure<ApiSettings>(configuration.GetSection(nameof(ApiSettings)));

            builder.Services.AddTransient<ViewModels.LoginViewModel>();

            return builder.Build();
        }
    }
}
