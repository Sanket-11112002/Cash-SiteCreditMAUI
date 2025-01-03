
//using CardGameCorner.Models;
//using CardGameCorner.Resources.Language;
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

//        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
//        [ObservableProperty]
//        private string welcomeMessage;


//        public HomeViewModel(IGameService gameService, ISecureStorage secureStorage)
//        {
//            UpdateLocalizedStrings();

//            // Listen for language changes
//            GlobalSettings.PropertyChanged += (s, e) =>
//            {
//                if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
//                {
//                    UpdateLocalizedStrings();
//                }
//            };
//            _gameService = gameService;
//            _secureStorage = secureStorage;
//            Games = new ObservableCollection<Game>();
//        }
//        private void UpdateLocalizedStrings()
//        {
//            WelcomeMessage = AppResources.WelcomeMessage; 
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

//        [RelayCommand]
//        private async Task GameSelectedAsync(Game game)
//        {
//            if (game == null) return;

//            // Navigate to GameDetailsPage and pass gameCode as a query parameter
//           await ((AppShell)Shell.Current).NavigateToGameDetails(game.GameCode);
//        }



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
        private readonly INavigationService _navigationService;
        private readonly GlobalSettingsService _globalSettings;

        [ObservableProperty]
        private bool isLoading;

        [ObservableProperty]
        private string errorMessage;

        [ObservableProperty]
        private ObservableCollection<Game> games;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string errorMsg;

        public HomeViewModel(IGameService gameService, ISecureStorage secureStorage, INavigationService navigationService)
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
            _globalSettings = GlobalSettingsService.Current;
            _gameService = gameService;
            _secureStorage = secureStorage;
            _navigationService = navigationService;
            Games = new ObservableCollection<Game>();
        }
        private void UpdateLocalizedStrings()
        {
            ErrorMsg = AppResources.APIErrorMessage;
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
              //  ErrorMessage = "Failed to load games. Please try again."; 
                ErrorMessage = ErrorMsg;
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

            // Save the selected game to secure storage
            //await _secureStorage.SetAsync("LastSelectedGame", game.GameCode);

            _globalSettings.SelectedGame = game.GameCode;

            await _secureStorage.SetAsync("LastSelectedGame", _globalSettings.SelectedGame);

            await _navigationService.NavigateToHomeAsync();
        }
    }
}