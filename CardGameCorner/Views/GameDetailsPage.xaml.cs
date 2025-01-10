using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using System.Diagnostics;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

namespace CardGameCorner.Views
{
    [QueryProperty(nameof(GameCode), "gameCode")]
    [QueryProperty(nameof(UiCode), "uiCode")]
    public partial class GameDetailsPage : ContentPage
    {
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
        private readonly GameDetailsViewModel _viewModel;
        private readonly ISecureStorage _secureStorage;
        private bool _isLoading;

        public string GameCode { get; set; }
        public string UiCode { get; set; }

        public GameDetailsPage(ISecureStorage secureStorage)
        {
            try
            {
                InitializeComponent();
                _secureStorage = secureStorage;
                _viewModel = new GameDetailsViewModel();
                BindingContext = _viewModel;

                carouselView.BackgroundColor = Colors.Transparent;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GameDetailsPage initialization error: {ex}");
            }
        }

        //protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        //{
        //    try
        //    {
        //        base.OnNavigatedTo(args);

        //        if (!string.IsNullOrEmpty(GameCode) && !string.IsNullOrEmpty(UiCode))
        //        {
        //            await LoadGameDetailsAsync();

        //            // Save the last selected game using secure storage
        //            if (!string.IsNullOrEmpty(GameCode))
        //            {
        //                await _secureStorage.SetAsync("LastSelectedGame", GameCode);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        System.Diagnostics.Debug.WriteLine($"OnNavigatedTo error: {ex}");
        //        await DisplayAlert("Error", "Failed to load game details.", "OK");
        //    }
        //}

        private async Task LoadGameDetailsAsync()
        {
            try
            {
                if (!_isLoading)
                {
                    // UiCode = GlobalSettings.SelectedLanguage;
                    UiCode = GlobalSettings.SelectedLanguage.Equals("English", StringComparison.OrdinalIgnoreCase)
                                 ? "en"
                                 : "it"; 


                    // Show loader overlay
                    loaderOverlay.IsVisible = true;
                    loadingIndicator.IsRunning = true;
                    _isLoading = true;

                    await _viewModel.LoadGameDetails(UiCode, GameCode);

                    // await _secureStorage.RemoveAsync("LastSelectedGame");

                    BindingContext = _viewModel;
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
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            //GlobalSettings.SelectedGame = "magic";
            Debug.WriteLine(GlobalSettings.SelectedGame);
            if (!string.IsNullOrEmpty(GlobalSettings.SelectedGame))
            {
                await _secureStorage.SetAsync("LastSelectedGame", GlobalSettings.SelectedGame);
                await _secureStorage.SetAsync("LastSelectedLang", GlobalSettings.SelectedLanguage);
            }

            // Fetch the last selected game from secure storage
            string lastSelectedGame = await _secureStorage.GetAsync("LastSelectedGame") ?? string.Empty;

            // If a last selected game is found, load its details
            if (!string.IsNullOrEmpty(lastSelectedGame))
            {
                GameCode = lastSelectedGame;
                await LoadGameDetailsAsync();
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

        private void OnPreviousClicked(object sender, EventArgs e)
        {
            if (carouselView.Position > 0)
            {
                carouselView.Position--;
            }
        }

        private void OnNextClicked(object sender, EventArgs e)
        {
            if (carouselView.Position < carouselView.ItemsSource.Cast<object>().Count() - 1)
            {
                carouselView.Position++;
            }
        }
        protected override bool OnBackButtonPressed()
        {

            Task<bool> answer = DisplayAlert(AppResources.Exit, AppResources.ExitApp, AppResources.YesMsg, "No");
            answer.ContinueWith(task =>
            {
                if (task.Result)
                {
                    Application.Current.Quit();
                }
            });
            return true;
        }
    }
}