using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CardGameCorner.Views;
using Microsoft.Extensions.Logging;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner
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

            // Register services
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ISecureStorage, SecureStorageService>();

            // Register pages and viewmodels
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddSingleton<AppShell>();

            // Register HttpClient
            builder.Services.AddHttpClient();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
