//using CardGameCorner.ViewModels;
//using Microsoft.Maui.Controls;

//namespace CardGameCorner.Views
//{
//    [QueryProperty(nameof(GameCode), "gameCode")]
//    [QueryProperty(nameof(UiCode), "uiCode")]
//    public partial class GameDetailsPage : ContentPage
//    {
//        private readonly GameDetailsViewModel _viewModel;

//        public string GameCode { get; set; }
//        public string UiCode { get; set; }

//        public GameDetailsPage()
//        {
//            InitializeComponent();
//            _viewModel = new GameDetailsViewModel();
//            BindingContext = _viewModel;
//        }

//        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
//        {
//            base.OnNavigatedTo(args);
//            if (!string.IsNullOrEmpty(GameCode) && !string.IsNullOrEmpty(UiCode))
//            {
//                await _viewModel.LoadGameDetails(UiCode, GameCode);
//            }
//        }

//        protected override void OnDisappearing()
//        {
//            base.OnDisappearing();
//            _viewModel.Dispose();
//        }

//        private async void CollectionView_RemainingItemsThresholdReached(object sender, EventArgs e)
//        {
//            // Implement infinite scrolling logic here if needed
//            await _viewModel.LoadGameDetails(UiCode, GameCode);
//        }
//    }
//}

using Microsoft.Maui.Controls;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views
{
    [QueryProperty(nameof(GameCode), "gameCode")]
    [QueryProperty(nameof(UiCode), "uiCode")]
    public partial class GameDetailsPage : ContentPage
    {
        private readonly GameDetailsViewModel _viewModel;
        private bool _isLoading;
        public string GameCode { get; set; }
        public string UiCode { get; set; }

        public GameDetailsPage()
        {
            try
            {
                InitializeComponent();
                _viewModel = new GameDetailsViewModel();
                BindingContext = _viewModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GameDetailsPage initialization error: {ex}");
            }
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            try
            {
                base.OnNavigatedTo(args);
                if (!string.IsNullOrEmpty(GameCode) && !string.IsNullOrEmpty(UiCode))
                {
                    await LoadGameDetailsAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"OnNavigatedTo error: {ex}");
                await DisplayAlert("Error", "Failed to load game details.", "OK");
            }
        }

        private async Task LoadGameDetailsAsync()
        {
            try
            {
                if (!_isLoading)
                {
                    // Show loader overlay
                    loaderOverlay.IsVisible = true;
                    loadingIndicator.IsRunning = true;

                    _isLoading = true;
                    await _viewModel.LoadGameDetails(UiCode, GameCode);

                    // Hide loader
                    loaderOverlay.IsVisible = false;
                    loadingIndicator.IsRunning = false;

                    _isLoading = false;
                }
            }
            catch (Exception ex)
            {
                _isLoading = false;

                // Hide loader in case of error
                loaderOverlay.IsVisible = false;
                loadingIndicator.IsRunning = false;

                System.Diagnostics.Debug.WriteLine($"LoadGameDetails error: {ex}");
                await DisplayAlert("Error", "Failed to load game details.", "OK");
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (_viewModel is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }

        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // Display dropdown-like popup for settings
            string result = await DisplayActionSheet("Settings", "Cancel", null,
                "Select Language", "Select Game");

            switch (result)
            {
                case "Select Language":
                    await HandleLanguageSelection();
                    break;

                case "Select Game":
                    await HandleGameSelection();
                    break;
            }
        }

        private async Task HandleLanguageSelection()
        {
            string language = await DisplayActionSheet("Choose a Language", "Cancel", null,
                "English", "Italian");

            if (!string.IsNullOrEmpty(language) && language != "Cancel")
            {
                Console.WriteLine($"Selected Language: {language}");
            }
        }

        private async Task HandleGameSelection()
        {
            string game = await DisplayActionSheet("Choose a Game", "Cancel", null,
                 "Pokémon", "One Piece", "Magic", "Yu-Gi-Oh!");

            if (!string.IsNullOrEmpty(game) && game != "Cancel")
            {
                Console.WriteLine($"Selected Game: {game}");
            }
        }
    }
}