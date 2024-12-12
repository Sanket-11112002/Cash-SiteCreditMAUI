//using CardGameCorner.Services;
//using CardGameCorner.Views;
//using System.Diagnostics;

//namespace CardGameCorner
//{
//    public partial class AppShell : Shell
//    {
//        private readonly IServiceProvider _serviceProvider;

//        public AppShell(IServiceProvider serviceProvider)
//        {
//            InitializeComponent();
//            _serviceProvider = serviceProvider;
//            RegisterRoutes();

//            // Set initial page
//            //GoToAsync($"//{nameof(LoginPage)}");

//            // GoToAsync("//HomePage");

//            // Check if there's a last selected game
//            string lastSelectedGame = GetLastSelectedGame();
//            if (!string.IsNullOrEmpty(lastSelectedGame))
//            {
//                // Navigate directly to the game detail page
//                NavigateToGameDetails(lastSelectedGame);
//            }
//            else
//            {
//                // Navigate to the home page
//                GoToAsync("//HomePage");
//            }

//        }
//        private string GetLastSelectedGame()
//        {
//            // Retrieve the last selected game from persistent storage
//            var preferences = Preferences.Default;
//            return preferences.Get("LastSelectedGame", string.Empty);
//        }

//        private void RegisterRoutes()
//        {
//            //Routing.RegisterRoute($"login", typeof(LoginPage));
//            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
//            Routing.RegisterRoute("//HomePage", typeof(HomePage));  // Register HomePage route
//           // Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));  // Register HomePage route
//            //Routing.RegisterRoute("GameDetailsPage", typeof(GameDetailsPage));
//            //Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
//            Routing.RegisterRoute("GameDetailsPage", typeof(CardGameCorner.Views.GameDetailsPage));

//            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
//            Routing.RegisterRoute(nameof(MyListPage), typeof(MyListPage));
//            Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
//            Routing.RegisterRoute(nameof(MyAccountPage), typeof(MyAccountPage));
//            Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
//            Routing.RegisterRoute(nameof(CardDetailPage), typeof(CardDetailPage));
//            Routing.RegisterRoute(nameof(CardComparisonPage), typeof(CardComparisonPage));
//        }

//        //private async void MyAccountTab_Navigated(object sender, ShellNavigatedEventArgs e)
//        //{
//        //    // Check if user is logged in before accessing MyAccountTab
//        //    if (!App.IsUserLoggedIn)
//        //    {
//        //        await GoToAsync("//login");
//        //    }
//        //}

//        protected override void OnNavigatedTo(NavigatedToEventArgs args)
//        {
//            base.OnNavigatedTo(args);

//            // Check if user is logged in when trying to access My Account
//            if (CurrentPage is MyAccountPage && !App.IsUserLoggedIn)
//            {
//                // Redirect to login if not logged in
//                GoToAsync("//login");
//            }
//        }

//        public async Task NavigateToGameDetails(string gameCode)
//        {
//            try
//            {
//                if (string.IsNullOrEmpty(gameCode))
//                {
//                    return;
//                }

//                await GameNavigationService.ShowGameDetailsPageAsync(gameCode);
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
//                await MainThread.InvokeOnMainThreadAsync(async () =>
//                {
//                    await DisplayAlert("Error", "Unable to open game details. Please try again.", "OK");
//                });
//            }
//        }
//    }
//}

using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CardGameCorner.Views;
using Microsoft.Maui.Controls;
using System;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner
{
    public partial class AppShell : Shell
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ISecureStorage _secureStorage;
        private readonly IAlertService _alertService;
        private readonly INavigationService _navigationService;
        private readonly AppShellViewModel _viewModel;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
        public AppShell(IServiceProvider serviceProvider, ISecureStorage secureStorage, IAlertService alertService, INavigationService navigationService)
        {
            InitializeComponent();
            _viewModel = new AppShellViewModel(GlobalSettingsService.Current);
            BindingContext = _viewModel;
            _serviceProvider = serviceProvider;
            _secureStorage = secureStorage;
            _alertService = alertService;
            _navigationService = navigationService;
            RegisterRoutes();

            // Determine initial navigation
            Loaded += OnShellLoaded;
        }

        private async void OnShellLoaded(object sender, EventArgs e)
        {
            await DetermineInitialNavigation();
        }

        //private async Task DetermineInitialNavigation()
        //{
        //    try
        //    {
        //        // Try to get the last selected game from secure storage
        //        string lastSelectedGame = await GetLastSelectedGameAsync();
        //        string lastSelectedlang = await GetLastSelectedLangAsync();
        //        string Jwttoken_User = await UserLoginCheck();
        //        GlobalSettings.SelectedGame = lastSelectedGame;
        //        GlobalSettings.SelectedLanguage = lastSelectedlang;
        //        if (!string.IsNullOrEmpty(Jwttoken_User))
        //        {
        //            App.IsUserLoggedIn = true;
        //        }
        //        if (!string.IsNullOrEmpty(lastSelectedGame))
        //        {
        //            // If a last selected game exists, navigate to its details
        //            //await NavigateToGameDetails(lastSelectedGame);
        //            await GoToAsync("//GameDetailsPage");
        //        }
        //        else
        //        {
        //           // If no last selected game, go to home page
        //            await GoToAsync("//HomePage");
        //            // await Shell.Current.GoToAsync(nameof(SettingsSlidePage));
        //            //var settingsViewModel = _serviceProvider.GetService<SettingsViewModel>();
        //           // var settingsPage = new SettingsSlidePage(settingsViewModel, _secureStorage, _alertService, _navigationService);
        //            ////var settingsPage = new SettingsSlidePage(settingsViewModel);
        //            //await Shell.Current.Navigation.PushModalAsync(settingsPage);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Fallback to home page in case of any navigation error
        //        System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
        //        //await GoToAsync("//HomePage");
        //        var settingsViewModel = _serviceProvider.GetService<SettingsViewModel>();
        //        var settingsPage = new SettingsSlidePage(settingsViewModel, _secureStorage, _alertService, _navigationService);
        //        //var settingsPage = new SettingsSlidePage(settingsViewModel);
        //        await Shell.Current.Navigation.PushModalAsync(settingsPage);
        //    }
        //}

        private async Task DetermineInitialNavigation()
        {
            try
            {
                // Try to get the last selected game from secure storage
                string lastSelectedGame = await GetLastSelectedGameAsync();
                string lastSelectedlang = await GetLastSelectedLangAsync();
                string jwtTokenUser = await UserLoginCheck();

                // Check if the token is expired
                if (!string.IsNullOrEmpty(jwtTokenUser) && !IsTokenExpired(jwtTokenUser))
                {
                    App.IsUserLoggedIn = true;
                }
                else
                {
                    App.IsUserLoggedIn = false;
                    jwtTokenUser = string.Empty;  // Clear the expired token
                }

                GlobalSettings.SelectedGame = lastSelectedGame;
                GlobalSettings.SelectedLanguage = lastSelectedlang;

                if (!string.IsNullOrEmpty(lastSelectedGame))
                {
                    // If a last selected game exists, navigate to its details
                    await GoToAsync("//GameDetailsPage");
                }
                else
                {
                    // If no last selected game, go to home page
                    await GoToAsync("//HomePage");
                }
            }
            catch (Exception ex)
            {
                // Fallback to home page in case of any navigation error
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
                var settingsViewModel = _serviceProvider.GetService<SettingsViewModel>();
                var settingsPage = new SettingsSlidePage(settingsViewModel, _secureStorage, _alertService, _navigationService);
                await Shell.Current.Navigation.PushModalAsync(settingsPage);
            }
        }

        // Decode JWT token and check expiration
        private bool IsTokenExpired(string token)
        {
            try
            {
                var jwt = new JwtSecurityToken(token); // Ensure you have Jwt.Net installed
                var exp = jwt.ValidTo;
                return exp < DateTime.UtcNow;
            }
            catch
            {
                return true; // If decoding fails, consider the token expired
            }
        }


        private async Task<string> GetLastSelectedGameAsync()
        {
            // Use secure storage to get the last selected game
            return await _secureStorage.GetAsync("LastSelectedGame") ?? string.Empty;
        }
        private async Task<string> UserLoginCheck()
        {
            // Use secure storage to get the last selected game
            return await _secureStorage.GetAsync("jwt_token") ?? string.Empty;
        }
        private async Task<string> GetLastSelectedLangAsync()
        {
            // Use secure storage to get the last selected game
            return await _secureStorage.GetAsync("LastSelectedLang") ?? string.Empty;
        }
        //private void RegisterRoutes()
        //{
        //    // Register all routes for the application
        //    Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
        //    Routing.RegisterRoute(nameof(HomePage), typeof(HomePage));
        //    //  Routing.RegisterRoute(nameof(GameDetailsPage), typeof(GameDetailsPage));
        //    Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
        //    Routing.RegisterRoute(nameof(MyListPage), typeof(MyListPage));
        //    Routing.RegisterRoute(nameof(SearchPage), typeof(SearchPage));
        //    Routing.RegisterRoute(nameof(MyAccountPage), typeof(MyAccountPage));
        //    Routing.RegisterRoute(nameof(ScanPage), typeof(ScanPage));
        //    Routing.RegisterRoute(nameof(CardDetailPage), typeof(CardDetailPage));
        //    Routing.RegisterRoute(nameof(CardComparisonPage), typeof(CardComparisonPage));
        //    Routing.RegisterRoute(nameof(SettingsSlidePage), typeof(SettingsSlidePage));
        //    // Routing.RegisterRoute(nameof(SearchQueryPage), typeof(SearchQueryPage));
        //      Routing.RegisterRoute("SearchQueryPage", typeof(SearchQueryPage));
        //      Routing.RegisterRoute("CardDetailPage", typeof(CardDetailPage));
        //}

        private void RegisterRoutes()
        {
            // Only register routes for pages not part of the Shell
            Routing.RegisterRoute(nameof(CardDetailPage), typeof(CardDetailPage));
            Routing.RegisterRoute(nameof(CardComparisonPage), typeof(CardComparisonPage));
            Routing.RegisterRoute(nameof(SettingsSlidePage), typeof(SettingsSlidePage));
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(RegistrationPage), typeof(RegistrationPage));
            Routing.RegisterRoute(nameof(SearchQueryPage), typeof(SearchQueryPage));
        }

        protected override void OnNavigatedTo(NavigatedToEventArgs args)
        {
            base.OnNavigatedTo(args);

            // Check if user is logged in when trying to access My Account
            if (CurrentPage is MyAccountPage && !App.IsUserLoggedIn)
            {
                // Redirect to login if not logged in
                GoToAsync("//login");
            }
        }

        public async Task NavigateToGameDetails(string gameCode)
        {
            try
            {
                if (string.IsNullOrEmpty(gameCode))
                {
                    // await GoToAsync("//HomePage");
                    //await Shell.Current.GoToAsync(nameof(SettingsSlidePage));
                    var settingsViewModel = _serviceProvider.GetService<SettingsViewModel>();
                    var settingsPage = new SettingsSlidePage(settingsViewModel, _secureStorage, _alertService, _navigationService);
                   // var settingsPage = new SettingsSlidePage(settingsViewModel);
                    await Shell.Current.Navigation.PushModalAsync(settingsPage);
                    return;
                }

                // Save the last selected game in secure storage
                await _secureStorage.SetAsync("LastSelectedGame", gameCode);

                // Navigate to game details
                await GameNavigationService.ShowGameDetailsPageAsync(gameCode);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex}");
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    await DisplayAlert("Error", "Unable to open game details. Redirecting to Home.", "OK");
                    // await GoToAsync("//HomePage");
                    // await Shell.Current.GoToAsync(nameof(SettingsSlidePage));
                    var settingsViewModel = _serviceProvider.GetService<SettingsViewModel>();
                    var settingsPage = new SettingsSlidePage(settingsViewModel, _secureStorage, _alertService, _navigationService);
                   // var settingsPage = new SettingsSlidePage(settingsViewModel);
                    await Shell.Current.Navigation.PushModalAsync(settingsPage);

                });
            }
        }

        private bool _settingsToolbarItemVisible = true;

        // Method to hide settings toolbar item
        public void HideSettingsToolbarItem()
        {
            if (_settingsToolbarItemVisible)
            {
                // Remove the toolbar item
                ToolbarItems.Remove(SettingsToolbarItem);
                _settingsToolbarItemVisible = false;
            }
        }

        // Method to show settings toolbar item
        public void ShowSettingsToolbarItem()
        {
            if (!_settingsToolbarItemVisible)
            {
                // Add the toolbar item back
                ToolbarItems.Add(SettingsToolbarItem);
                _settingsToolbarItemVisible = true;
            }
        }
        //private async void OnSettingsClicked(object sender, EventArgs e)
        //{
        //    // Navigate to settings page using Shell navigation
        //    await GoToAsync(nameof(SettingsSlidePage));
        //}

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            var settingsViewModel = _serviceProvider.GetService<SettingsViewModel>();
            var settingsPage = new SettingsSlidePage(settingsViewModel, _secureStorage, _alertService, _navigationService);
           // var settingsPage = new SettingsSlidePage(settingsViewModel);
            await Shell.Current.Navigation.PushModalAsync(settingsPage);
        }
        protected override void OnNavigating(ShellNavigatingEventArgs args)
        {
            base.OnNavigating(args);
            // Your custom navigation logic here
        }
    }
}