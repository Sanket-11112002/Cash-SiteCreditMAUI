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
                    _isLoading = true;
                    await _viewModel.LoadGameDetails(UiCode, GameCode);
                    _isLoading = false;
                }
            }
            catch (Exception ex)
            {
                _isLoading = false;
                System.Diagnostics.Debug.WriteLine($"LoadGameDetails error: {ex}");
                throw;
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

      
    }

}


