using System.Runtime.InteropServices;
using System.Text.Json;
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
        private string searchResultImage;


        [ObservableProperty]
        public ImageSource scannedImage;


        [ObservableProperty]
        public CardSearchResponseViewModel responseContent;



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
            
        }

        //public void Initialize(ApiResponse_Card response, ImageSource image)
        //{
        //    ResponseContent = response;
        //    ScannedImage = image;

        //    if (ResponseContent?.Result != null)
        //    {
        //        CardName = ResponseContent.Result.Title;
        //        CardRarity = ResponseContent.Result.Rarity;
        //        CardSet = ResponseContent.Result.Set;


        //    }
        //}

        public void Initialize(CardSearchResponseViewModel response, ImageSource image)
        {
            responseContent = response;
            scannedImage = image;

            if (responseContent?.Products[0] != null)
            {
                CardName = responseContent.Products[0].Model;
                CardRarity = responseContent.Products[0].Rarity;
                CardSet = responseContent.Products[0].Category;

                searchResultImage= "https://www.cardgamecorner.com"+responseContent.Products[0].Image;
            
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
            try
            {
                // Ensure the responseContent is not null
                if (responseContent?.Products?.Count > 0)
                {
                    // Get the first product
                    var product = responseContent.Products[0];

                    // Deserialize the Variants JSON string into a list of ProductVariant1 objects
                    var variants = product.Variants;

                    if (variants != null)
                    {
                        // Extract distinct languages and conditions
                        var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
                        var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

                        // For testing, log the results to the console
                        Console.WriteLine("Languages:");
                        foreach (var lang in distinctLanguages)
                        {
                            Console.WriteLine(lang);
                        }

                        Console.WriteLine("Conditions:");
                        foreach (var condition in distinctConditions)
                        {
                            Console.WriteLine(condition);
                        }

                        var details = new CardDetailViewModel
                        {
                            Languages = distinctLanguages,
                            Conditions = distinctConditions,
                            Name = product.Model,
                            Rarity = product.Rarity,
                            Category = product.Category,
                            ImageUrl = "https://www.cardgamecorner.com" + product.Image,
                            game = product.Game
                        };

                        // Navigate to the CardDetailsPage
                       

                        await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));



                    }

                    // Show success confirmation
                    await Application.Current.MainPage.DisplayAlert("Success",
                        "Card added to your collection!", "OK");
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "No product data found in the response!", "OK");
                }
            }
            catch (Exception ex)
            {
                // Handle errors gracefully
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"An error occurred: {ex.Message}", "OK");
            }
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