//namespace CardGameCorner
//{
//    public partial class App : Application
//    {
//        private readonly IServiceProvider _serviceProvider;
//        public static bool IsUserLoggedIn { get; set; } = false;
//        public App(IServiceProvider serviceProvider)
//        {
//            InitializeComponent();
//            _serviceProvider = serviceProvider;
//            MainPage = new AppShell(_serviceProvider);


//        }

//        public void RestartAppForNewSession()
//        {
//            // Reset MainPage to a new AppShell instance with a clean navigation stack
//            MainPage = new AppShell(_serviceProvider);

//            // Navigate to LoginPage directly to start a new session
//            Shell.Current.GoToAsync("login");
//        }

//        protected override Window CreateWindow(IActivationState activationState)
//        {
//            Window window = base.CreateWindow(activationState);

//            // Customize the window size for desktop platforms
//            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI ||
//                DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
//            {
//                window.Width = 400;
//                window.Height = 600;
//            }

//            return window;
//        }
//    }
//}


using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISecureStorage _secureStorage;
        private readonly IAlertService _alertService;
        private readonly INavigationService _navigationService;

        public static bool IsUserLoggedIn { get; set; } = false;

        public App(IServiceProvider serviceProvider, ISecureStorage secureStorage, IAlertService alertService, INavigationService navigationService)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _secureStorage = secureStorage;
            _alertService = alertService;
            _navigationService = navigationService;

            // Create MainPage with both serviceProvider and secureStorage
            MainPage = new AppShell(_serviceProvider, _secureStorage, _alertService, _navigationService);
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

            // Check internet connection on app startup
            CheckInternetConnection();

            // Initialize language from GlobalSettings
            var language = GlobalSettingsService.Current.SelectedLanguage ?? "English";
            GlobalSettingsService.Current.SelectedLanguage = language; // Ensures OnLanguageChanged triggers
        }

        private void CheckInternetConnection()
        {
            var connectivity = Connectivity.Current;
            if (connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                ShowNoInternetMessage();
            }

            // Subscribe to connectivity changes
            Connectivity.ConnectivityChanged += Connectivity_ConnectivityChanged;
        }

        private async void Connectivity_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            if (e.NetworkAccess == NetworkAccess.Internet)
            {
                // Internet is restored
                //  await Application.Current.MainPage.DisplayAlert("Internet Restored", "Internet connection has been restored.", "OK");
                await Application.Current.MainPage.DisplayAlert(AppResources.Internet_Restored, AppResources.Internet_connection_restored, AppResources.OK);

                // Optionally refresh the app or reload the necessary content
                await RestartAppForNewSession();
            }
            else
            {
                // Internet is lost
                ShowNoInternetMessage();
            }
        }

        private async void ShowNoInternetMessage()
        {
            // Keep showing the message until the user enables internet
            while (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
            {
                // await Application.Current.MainPage.DisplayAlert("No Internet", "Please enable internet connection.", "OK");
                await Application.Current.MainPage.DisplayAlert(AppResources.No_Internet, AppResources.Enable_internet_connection, AppResources.OK);
                await Task.Delay(5000); // Recheck every 5 seconds
            }
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            Window window = base.CreateWindow(activationState);

            // Customize the window size for desktop platforms
            if (DeviceInfo.Current.Platform == DevicePlatform.WinUI ||
                DeviceInfo.Current.Platform == DevicePlatform.MacCatalyst)
            {
                window.Width = 400;
                window.Height = 600;
            }

            return window;
        }
    }
}
