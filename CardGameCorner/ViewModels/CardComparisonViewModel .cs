using System.Runtime.InteropServices;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace CardGameCorner.ViewModels
{
    public partial class CardComparisonViewModel : ObservableObject
    {
        [ObservableProperty]
        private string cardName;

        [ObservableProperty]
        private string cardRarity;

        [ObservableProperty]
        private string cardSet;

        [ObservableProperty]
        private ImageSource searchResultImage;

        public CardComparisonViewModel()
        {
            // Initialize default values
            CardName = "Blue-Eyes White Dragon";
            CardRarity = "Secret Rare";
            CardSet = "RAGE OF THE ABYSS";
        }

        //[RelayCommand]
        //private async Task CaptureImage()
        //{
        //    try
        //    {
        //        var result = await MediaPicker.CapturePhotoAsync();
        //        if (result != null)
        //        {
        //            // Handle the captured image
        //            var stream = await result.OpenReadAsync();
        //            // Send to your card scanning service
        //            await ProcessCapturedImage(stream);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"Failed to capture image: {ex.Message}", "OK");
        //    }
        //}

        //[RelayCommand]
        //private async Task ConfirmCard()
        //{
        //    // Add card to user's collection
        //    await Application.Current.MainPage.DisplayAlert("Success",
        //        "Card added to your collection!", "OK");
        //    // Navigate back or refresh
        //}

        //[RelayCommand]
        //private async Task Retry()
        //{
        //    // Clear current results and restart capture process
        //    await CaptureImage();
        //}

        //private async Task ProcessCapturedImage(Stream imageStream)
        //{
        //    try
        //    {
        //        //var scanService = new ScanCardService();
        //        //await scanService.UploadImageAsync(imageStream);
        //        // Handle the response and update the UI
        //    }
        //    catch (Exception ex)
        //    {
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"Failed to process image: {ex.Message}", "OK");
        //    }
        //}
    }
}