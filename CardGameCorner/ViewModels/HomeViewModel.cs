//using CardGameCorner.Models;
//using CardGameCorner.Services;
//using CardGameCorner.Views;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
//using System.Collections.ObjectModel;
//using System.Diagnostics;
//using System.Windows.Input;
//using ISecureStorage = CardGameCorner.Services.ISecureStorage;

//namespace CardGameCorner.ViewModels
//{
//    public partial class HomeViewModel : ObservableObject
//    {
//        private readonly IGameService _gameService;
//        private readonly ISecureStorage _secureStorage;

//        [ObservableProperty]
//        private bool isLoading;

//        [ObservableProperty]
//        private string errorMessage;

//        [ObservableProperty]
//        private ObservableCollection<Game> games;

//        //public ICommand LogoutCommand { get; }

//        public HomeViewModel(IGameService gameService, ISecureStorage secureStorage)
//        {
//            _gameService = gameService;
//            _secureStorage = secureStorage;
//            Games = new ObservableCollection<Game>();
//            //LogoutCommand = new Command(OnLogout);
//        }

//        [RelayCommand]
//        private async Task LoadGamesAsync()
//        {
//            try
//            {
//                if (IsLoading) return;
//                IsLoading = true;
//                ErrorMessage = string.Empty;

//                var gamesList = await _gameService.GetGamesAsync();
//                Games.Clear();
//                foreach (var game in gamesList)
//                {
//                    Games.Add(game);
//                }
//            }
//            //catch (UnauthorizedAccessException)
//            //{
//            //    await HandleUnauthorizedAccess();
//            //}
//            catch (Exception ex)
//            {
//                ErrorMessage = "Failed to load games. Please try again.";
//                Debug.WriteLine($"Error: {ex.Message}");
//            }
//            finally
//            {
//                IsLoading = false;
//            }
//        }

//        //private async Task HandleUnauthorizedAccess()
//        //{
//        //    try
//        //    {
//        //        // Clear the stored token
//        //        _secureStorage.Remove("jwt_token");

//        //        // Show message to user
//        //        await Application.Current.MainPage.DisplayAlert(
//        //            "Session Expired",
//        //            "Your session has expired. Please login again.",
//        //            "OK");

//        //        // Navigate back to login page
//        //        await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Debug.WriteLine($"Error handling unauthorized access: {ex.Message}");
//        //        ErrorMessage = "Session expired. Please restart the application.";
//        //    }
//        //}

//        //[RelayCommand]
//        //private async Task GameSelectedAsync(Game game)
//        //{
//        //    if (game == null) return;
//        //    await Shell.Current.GoToAsync($"gameDetails?gameCode={game.GameCode}");
//        //}

//        [RelayCommand]
//        private async Task GameSelectedAsync(Game game)
//        {
//            if (game == null) return;

//            // Navigate to GameDetailsPage and pass gameCode as a query parameter
//            await ((AppShell)Shell.Current).NavigateToGameDetails(game.GameCode);
//        }

//        //private async void OnLogout()
//        //{
//        //    try
//        //    {
//        //        // Clear stored token if needed
//        //        _secureStorage.Remove("jwt_token");

//        //        // Reset the application to start a new session
//        //        ((App)Application.Current).RestartAppForNewSession();
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Debug.WriteLine($"Error during logout: {ex.Message}");
//        //    }
//        //}


//    }
//}

using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private readonly IGameService _gameService;
        private readonly ISecureStorage _secureStorage;


        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private ObservableCollection<Game> games;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
        [ObservableProperty]
        private string welcomeMessage;
       

        public HomeViewModel(IGameService gameService, ISecureStorage secureStorage)
        {
            UpdateLocalizedStrings();

            // Listen for language changes
            GlobalSettings.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
                {
                    UpdateLocalizedStrings();
                }
            };
            _gameService = gameService;
            _secureStorage = secureStorage;
            Games = new ObservableCollection<Game>();
        }
        private void UpdateLocalizedStrings()
        {
            WelcomeMessage = AppResources.WelcomeMessage; 
        }

       

        [RelayCommand]
        private async Task LoadGamesAsync()
        {
            try
            {
                if (IsLoading) return;
                IsLoading = true;
                ErrorMessage = string.Empty;

                var gamesList = await _gameService.GetGamesAsync();
                Games.Clear();
                foreach (var game in gamesList)
                {
                    Games.Add(game);
                }
            }
           
            catch (Exception ex)
            {
                ErrorMessage = "Failed to load games. Please try again.";
                Debug.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task GameSelectedAsync(Game game)
        {
            if (game == null) return;

            GlobalSettings.SelectedGame = game.GameCode;

            Preferences.Set("IsGameSelected", true);
            Preferences.Set("SelectedGame", game.GameCode);



            // Navigate to GameDetailsPage and pass gameCode as a query parameter
            await ((AppShell)Shell.Current).NavigateToGameDetails(game.GameCode);
        }

      

    }
}