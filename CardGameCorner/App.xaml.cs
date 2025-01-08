
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CardGameCorner.Views;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISecureStorage _secureStorage;
        private readonly IAlertService _alertService;
        private readonly INavigationService _navigationService;
        private readonly IGameService _gameService;
        private readonly GlobalSettingsService _globalSettingsService;

        public static bool IsUserLoggedIn { get; set; } = false;

        public App(IServiceProvider serviceProvider, ISecureStorage secureStorage, IAlertService alertService, INavigationService navigationService, GlobalSettingsService globalSettingsService, IGameService gameService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _secureStorage = secureStorage;
            _alertService = alertService;
            _navigationService = navigationService;
            _globalSettingsService = globalSettingsService; 
            _gameService = gameService;

            var lastSelectedGame = Task.Run(() => _secureStorage.GetAsync("LastSelectedGame")).Result ?? string.Empty;
            var lastSelectedLang = Task.Run(() => _secureStorage.GetAsync("LastSelectedLang")).Result ?? "English";
            _globalSettingsService.SelectedGame = lastSelectedGame;
            _globalSettingsService.SelectedLanguage = lastSelectedLang;

            if (!string.IsNullOrEmpty(lastSelectedGame))
            {
                MainPage = new AppShell(_serviceProvider, _secureStorage, _alertService, _navigationService);
            }
            else
            {
                HomeViewModel homeViewModel = new HomeViewModel(_gameService, _secureStorage, _navigationService, this);
                MainPage = new HomePage(homeViewModel);
            }

        }

        public async Task RestartAppForNewSession()
        {
            // Clear the last selected game when restarting
            // await _secureStorage.RemoveAsync("LastSelectedGame");

            // Reset MainPage to a new AppShell instance
            MainPage = new AppShell(_serviceProvider, _secureStorage, _alertService, _navigationService);

            // Navigate to LoginPage directly to start a new session
            //  await Shell.Current.GoToAsync("login");
        }

        protected override void OnStart()
        {
            base.OnStart();
            CheckInternetConnection();

            var language = GlobalSettingsService.Current.SelectedLanguage ?? "English";
            GlobalSettingsService.Current.SelectedLanguage = language;
        }

        private void CheckInternetConnection()
        {
            var connectivity = Connectivity.Current;
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                ShowNoInternetMessage();
            }

            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                // Get saved language before showing message
                var savedLanguage = await _secureStorage.GetAsync("LastSelectedLang") ?? "English";
                GlobalSettingsService.Current.SelectedLanguage = savedLanguage;

                await Application.Current.MainPage.DisplayAlert(AppResources.Internet_Restored,
                    AppResources.Internet_connection_restored, AppResources.OK);

                await RestartAppForNewSession();
            }
            else
            {
                ShowNoInternetMessage();
            }
        }

        private async void ShowNoInternetMessage()
        {
            // Get saved language before showing message
            var savedLanguage = await _secureStorage.GetAsync("LastSelectedLang") ?? "English";
            GlobalSettingsService.Current.SelectedLanguage = savedLanguage;

            while (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert(AppResources.No_Internet,
                    AppResources.Enable_internet_connection, AppResources.OK);
                await Task.Delay(5000);
            }
        }

    }
}
