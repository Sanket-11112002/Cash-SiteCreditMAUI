using System.Text.Json;
using CardGameCorner.Models;
using CardGameCorner.ViewModels;


using CardGameCorner.Services;
using Microsoft.Maui.Controls;


namespace CardGameCorner.Views;

public partial class SearchQueryPage : ContentPage
{
    private readonly IScanCardService _service;
    public SearchQueryPage()
	{
		InitializeComponent();

	}

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // Use the global settings service to show settings
        var globalSettings = GlobalSettingsService.Current;

        string result = await DisplayActionSheet(
            "Settings",
            "Cancel",
            null,
            "Select Language",
            "Select Game");

        switch (result)
        {
            case "Select Language":
                await globalSettings.ChangeLanguageAsync();
                break;
            case "Select Game":
                await globalSettings.ChangeGameAsync();
                break;
        }
    }



 
    private async void OnUploadButtonClicked(object sender, EventArgs e)
    {
        try
        {
            // Get the ImageButton that was clicked
            var imageButton = sender as ImageButton;
            if (imageButton == null)
            {
                await DisplayAlert("Error", "The ImageButton is null.", "OK");
                return;
            }

            // Retrieve the Card object bound to the ImageButton via CommandParameter
            var selectedCard = imageButton?.CommandParameter as Product;
            if (selectedCard == null)
            {
                await DisplayAlert("Error", "Selected card object is not found.", "OK");
                return;
            }

            // Get the image URL from the selected card
            string imageUrl = selectedCard.Image;

            // Check if the image URL is valid
            if (string.IsNullOrEmpty(imageUrl))
            {
                await DisplayAlert("Error", "No image URL available.", "OK");
                return;
            }

            // Download the image from the URL
            var imageBytes = await DownloadImageAsync(imageUrl);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                await DisplayAlert("Error", "Failed to download image.", "OK");
                return;
            }

            // Proceed with image compression or upload if necessary
            Console.WriteLine("Image downloaded successfully!");

            var viewModel = BindingContext as SearchViewModel;
            if (viewModel == null)
            {
                await DisplayAlert("Error", "ViewModel is not set.", "OK");
                return;
            }

            var compressedImageStream = await viewModel.CompressImageAsync(new MemoryStream(imageBytes), 100 * 1024);

            // Clone the compressed stream for upload
            var uploadStream = new MemoryStream();
            compressedImageStream.Position = 0; // Reset position for upload
            await compressedImageStream.CopyToAsync(uploadStream);
            uploadStream.Position = 0; // Reset for upload

            // Upload the image
            //var apiResponse = await viewModel.UploadImageAsync(uploadStream);

            
                //Console.WriteLine($"Upload successful: {apiResponse}");

                var cardRequest = new CardSearchRequest
                {
                    Title = selectedCard.Model,
                    Set = selectedCard.SetCode, // Assuming the card has a `Set` property
                    Game = selectedCard.Game, // Assuming the card has a `Game` property
                    Lang = "en", // You can use a language property if needed
                    Foil = 0, // Assuming you don't need foil info
                    FirstEdition = 0 // Assuming you don't need first edition info
                };

                // Search for the card based on the uploaded image
                CardComparisonViewModel data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(uploadStream.ToArray())));

                if (data != null && data.responseContent.Products != null)
                {
                    var detailslst=new List<CardDetailViewModel>();
                    foreach (var product in data.responseContent.Products)
                    {
                        // Deserialize the Variants JSON string into a list of ProductVariant1 objects
                        var variants = product.Variants;

                        if (variants != null)
                        {
                            // Extract distinct languages and conditions
                            var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
                            var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

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

                            var details = new CardDetailViewModel()
                            {
                                Languages = distinctLanguages,
                                Conditions = distinctConditions,
                                Name = product.Model,
                                Rarity = product.Rarity,
                                Category = product.Category,
                                ImageUrl = "https://www.cardgamecorner.com"+product.Image,
                                game = product.Game
                            };
                        detailslst.Add(details);


                        // Navigate to the CardDetailsPage for each product

                        }
                  
                        else
                        {
                            Console.WriteLine("No variants found for product.");
                        }
                    }
                     await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(detailslst));
            }
                else
                {
                    await DisplayAlert("Error", "No products found.", "OK");
                }
            
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to fetch or upload image: {ex.Message}", "OK");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    private async Task<byte[]> DownloadImageAsync(string imageUrl)
    {
        try
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(imageUrl);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsByteArrayAsync();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error downloading image: {ex.Message}");
        }
        return null;
    }



    private void OnFavoriteButtonClicked(object sender, EventArgs e)
    {
         DisplayAlert("Cardd", "favouritee", "OK");
       
    }
}