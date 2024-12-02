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


using CardGameCorner.Services;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner
{
    public partial class App : Application
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISecureStorage _secureStorage;

        public static bool IsUserLoggedIn { get; set; } = false;

        public App(IServiceProvider serviceProvider, ISecureStorage secureStorage)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _secureStorage = secureStorage;

            // Create MainPage with both serviceProvider and secureStorage
            MainPage = new AppShell(_serviceProvider, _secureStorage);
        }

        public async Task RestartAppForNewSession()
        {
            // Clear the last selected game when restarting
            await _secureStorage.RemoveAsync("LastSelectedGame");

            // Reset MainPage to a new AppShell instance
            MainPage = new AppShell(_serviceProvider, _secureStorage);

            // Navigate to LoginPage directly to start a new session
            await Shell.Current.GoToAsync("login");
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
