using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CardGameCorner.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Compatibility.Hosting;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;
using CommunityToolkit.Maui;
using CardGameCorner.Models;

namespace CardGameCorner
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitCamera()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register services
            builder.Services.AddSingleton<IAuthService, AuthService>();
            builder.Services.AddSingleton<ISecureStorage, SecureStorageService>();
            builder.Services.AddSingleton<IScanCardService, ScanCardService>();
            builder.Services.AddSingleton<SQLiteService>();
            // Register pages and viewmodels
            builder.Services.AddSingleton<IGameService, GameService>();
            builder.Services.AddSingleton<HomeViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<HomePage>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddTransient<RegistrationPage>();
            builder.Services.AddTransient<CardComparisonPage>();
            builder.Services.AddTransient<MyListPage>();
            builder.Services.AddTransient<MyListViewModel>();
            builder.Services.AddTransient<MyAccountPage>();
            builder.Services.AddTransient<SearchPage>();
            builder.Services.AddTransient<ScanPage>();
            builder.Services.AddTransient<GameDetailsViewModel>();
            builder.Services.AddTransient<GameDetailsPage>();
            builder.Services.AddTransient<ScanCardViewModel>();
            builder.Services.AddTransient<SearchViewModel>();
            builder.Services.AddTransient<SearchQueryPage>();
            builder.Services.AddTransient<CardDetailPage>();
            builder.Services.AddTransient<SettingsSlidePage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddTransient<CardDetailViewModel>();
            builder.Services.AddTransient<ProductList>();
            builder.Services.AddTransient<cardDetailRequest>();

            builder.Services.AddSingleton<IMyAccountService, MyAccountService>();
            builder.Services.AddTransient<MyAccountViewModel>();
            builder.Services.AddTransient<CardComparisonViewModel>();
            builder.Services.AddTransient<ApiResponse_Card>();
            builder.Services.AddTransient<ScannedCardDetails>();

            builder.Services.AddSingleton<IAlertService, AlertService>();
            builder.Services.AddSingleton<INavigationService, NavigationService>();
            builder.Services.AddSingleton<IListboxService,ListBoxService>();

            // Register GlobalSettingsService as a singleton
            builder.Services.AddSingleton(GlobalSettingsService.Current);


            // Register HttpClient
            builder.Services.AddHttpClient();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}