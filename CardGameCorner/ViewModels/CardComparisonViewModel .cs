using System.Runtime.InteropServices;
using System.Windows.Input;
using CardGameCorner.Models;
using CardGameCorner.Views;
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


        [ObservableProperty]
        public ImageSource scannedImage;


        [ObservableProperty]
        public ApiResponse_Card responseContent;



        //public CardComparisonViewModel()
        //{
        //    // Initialize default values
        //    CardName = "Blue-Eyes White Dragon";
        //    CardRarity = "Secret Rare";
        //    CardSet = "RAGE OF THE ABYSS";

        //}
        //public CardComparisonViewModel(ApiResponse_Card responseContent, ImageSource scannedImage)
        //{
        //    // Assign the passed data to properties
        //    ResponseContent = responseContent;
        //    ScannedImage = scannedImage;

        //    // Map data from the response to individual properties
        //    if (ResponseContent?.Result != null)
        //    {
        //        CardName = ResponseContent.Result.Title;
        //        CardRarity = ResponseContent.Result.Rarity;
        //        CardSet = ResponseContent.Result.Set;
        //    }
        //}

        public CardComparisonViewModel()
        {
            // Default initialization if needed
        }

        public void Initialize(ApiResponse_Card response, ImageSource image)
        {
            ResponseContent = response;
            ScannedImage = image;

            if (ResponseContent?.Result != null)
            {
                CardName = ResponseContent.Result.Title;
                CardRarity = ResponseContent.Result.Rarity;
                CardSet = ResponseContent.Result.Set;


            }
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

        [RelayCommand]
        private async Task ConfirmCard()
        {
            // Add card to user's collection
            await Application.Current.MainPage.DisplayAlert("Success",
                "Card added to your collection!", "OK");
            // Navigate back or refresh
        }

        

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