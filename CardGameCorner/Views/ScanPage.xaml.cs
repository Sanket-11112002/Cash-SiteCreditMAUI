using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace CardGameCorner.Views
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();

            // Inject the ViewModel with the ApiService
            BindingContext = new ScanCardViewModel(new ScanCardService());
        }

        private async void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            var viewModel = BindingContext as ScanCardViewModel;
            if (viewModel != null)
            {
                await viewModel.CaptureImageCommand.ExecuteAsync(e);
            }
        }
        private async void OnCaptureButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Capture the image
                await cameraView.CaptureImage(CancellationToken.None);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Capture Error", $"Failed to capture image: {ex.Message}", "OK");
            }
        }

    }
}
