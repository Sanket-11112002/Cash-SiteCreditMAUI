using System.Text.Json;
using CardGameCorner.Models;
using CardGameCorner.ViewModels;


using CardGameCorner.Services;

namespace CardGameCorner.Views;

public partial class SearchQueryPage : ContentPage
{

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



    //private async void OnUploadButtonClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Get the image URL bound to the ImageButton
    //        var viewModel = BindingContext as SearchViewModel;
    //        if (viewModel == null)
    //        {
    //            await DisplayAlert("Error", "ViewModel is not set.", "OK");
    //            return;
    //        }

    //        //string imageUrl = viewModel.Products[0].Image; // Ensure this is set in the ViewModel
    //        string imageUrl = "https://www.cardgamecorner.com/prodotti/singles/pokemon/mew/charizard-ex_733794.jpg"; // Ensure this is set in the ViewModel

    //        if (string.IsNullOrEmpty(imageUrl))
    //        {
    //            await DisplayAlert("Error", "No image URL available.", "OK");
    //            return;
    //        }

    //        // Fetch the image from the URL
    //        var imageBytes = await DownloadImageAsync(imageUrl);
    //        if (imageBytes == null || imageBytes.Length == 0)
    //        {
    //            await DisplayAlert("Error", "Failed to download image.", "OK");
    //            return;
    //        }

    //        // Compress the image
    //        var compressedImageStream = await viewModel.CompressImageAsync(new MemoryStream(imageBytes), 100 * 1024);

    //        // Clone the compressed stream for display
    //        var displayStream = new MemoryStream();
    //        compressedImageStream.Position = 0; // Reset position for display
    //        await compressedImageStream.CopyToAsync(displayStream);
    //        displayStream.Position = 0; // Reset for display

    //        // Clone the compressed stream for upload
    //        var uploadStream = new MemoryStream();
    //        compressedImageStream.Position = 0; // Reset position for upload
    //        await compressedImageStream.CopyToAsync(uploadStream);
    //        uploadStream.Position = 0; // Reset for upload

    //        // Upload the image (uploadStream is passed to UploadImageAsync)
    //        var apiResponse = await viewModel.UploadImageAsync(uploadStream);

    //        if (apiResponse != null)
    //        {
    //            Console.WriteLine($"Upload successful: {apiResponse}");

    //            var cardRequest = new CardSearchRequest
    //            {
    //                Title = "Angel of Mercy", // Example fields, update with actual API response
    //                Set = "IMA",
    //                Game = "magic",
    //                Lang = "en",
    //                Foil = 0,
    //                FirstEdition = 0
    //            };

    //            CardComparisonViewModel data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

    //            if (data != null)
    //            {
    //                //await Navigation.PushAsync(new CardComparisonPage(data));

    //                if (data.responseContent.Products.Count > 0)
    //                {
    //                    // Get the first product
    //                    var product = data.responseContent.Products[0];

    //                    // Deserialize the Variants JSON string into a list of ProductVariant1 objects
    //                    var variants = product.Variants;

    //                    if (variants != null)
    //                    {
    //                        // Extract distinct languages and conditions
    //                        var distinctLanguages = variants.Select(v => v.Language).Distinct().ToList();
    //                        var distinctConditions = variants.Select(v => v.Condition).Distinct().ToList();

    //                        // For testing, log the results to the console
    //                        Console.WriteLine("Languages:");
    //                        foreach (var lang in distinctLanguages)
    //                        {
    //                            Console.WriteLine(lang);
    //                        }

    //                        Console.WriteLine("Conditions:");
    //                        foreach (var condition in distinctConditions)
    //                        {
    //                            Console.WriteLine(condition);
    //                        }

    //                        var details = new CardDetailViewModel
    //                        {
    //                            Languages = distinctLanguages,
    //                            Conditions = distinctConditions,
    //                            Name = product.Model,
    //                            Rarity = product.Rarity,
    //                            Category = product.Category,
    //                           // ImageUrl = "https://www.cardgamecorner.com" + product.Image,
    //                            ImageUrl = imageUrl,
    //                            game = product.Game
    //                        };

    //                        // Navigate to the CardDetailsPage


    //                        await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));

    //                    }
    //                    else
    //                    {
    //                        await DisplayAlert("Error", "Something went wrong", "OK");
    //                    }
    //                }
    //                else
    //                {
    //                    await DisplayAlert("Error", "Card not found", "OK");
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        await DisplayAlert("Error", $"Failed to fetch or upload image: {ex.Message}", "OK");
    //        Console.WriteLine($"Error: {ex.Message}");
    //    }
    //}


    private async void OnUploadButtonClicked(object sender, EventArgs e)
    {
        // Show loading overlay
        SetLoadingState(true);

        try
        {
            var viewModel = BindingContext as SearchViewModel;
            if (viewModel == null)
            {
                await DisplayAlert("Error", "ViewModel is not set.", "OK");
                SetLoadingState(false);
                return;
            }

            string imageUrl = "https://www.cardgamecorner.com/prodotti/singles/pokemon/mew/charizard-ex_733794.jpg";
          //  string imageUrl = "https://www.cardgamecorner.com/prodotti/singles/pokemon/mew/charizard-ex_733794.jpg";

            if (string.IsNullOrEmpty(imageUrl))
            {
                await DisplayAlert("Error", "No image URL available.", "OK");
                SetLoadingState(false);
                return;
            }

            // Fetch the image from the URL
            var imageBytes = await DownloadImageAsync(imageUrl);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                await DisplayAlert("Error", "Failed to download image.", "OK");
                SetLoadingState(false);
                return;
            }

            // Compress the image
            var compressedImageStream = await viewModel.CompressImageAsync(new MemoryStream(imageBytes), 100 * 1024);

            // Clone the compressed stream for upload
            var uploadStream = new MemoryStream();
            compressedImageStream.Position = 0;
            await compressedImageStream.CopyToAsync(uploadStream);
            uploadStream.Position = 0;

            // Upload the image
            var apiResponse = await viewModel.UploadImageAsync(uploadStream);
            if (apiResponse != null)
            {
                Console.WriteLine($"Upload successful: {apiResponse}");
                var cardRequest = new CardSearchRequest
                {
                    Title = "Angel of Mercy",
                    Set = "IMA",
                    Game = "magic",
                    Lang = "en",
                    Foil = 0,
                    FirstEdition = 0
                };

                // Perform card search
               // CardComparisonViewModel data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));
                CardComparisonViewModel data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(uploadStream.ToArray())));

                if (data != null && data.responseContent.Products != null)
                {
                    foreach (var product in data.responseContent.Products)
                    {
                        // Deserialize the Variants JSON string into a list of ProductVariant1 objects
                        var variants = product.Variants;

                    if (variants != null)
                    {
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

                            var details = new CardDetailViewModel
                            {
                                Languages = distinctLanguages,
                                Conditions = distinctConditions,
                                Name = product.Model,
                                Rarity = product.Rarity,
                                Category = product.Category,
                                ImageUrl = imageUrl,
                                game = product.Game
                            };

                            // Navigate to the CardDetailsPage for each product
                            await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));
                        }
                        else
                        {
                            Console.WriteLine("No variants found for product.");
                        }
                    }
                }
                else
                {
                    await DisplayAlert("Error", "No products found.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            SetLoadingState(false);
            await DisplayAlert("Error", $"Failed to fetch or upload image: {ex.Message}", "OK");
            Console.WriteLine($"Error: {ex.Message}");
        }
    }

    // New method to manage loading state
    private void SetLoadingState(bool isLoading)
    {
        // This assumes you have a loading overlay in your XAML 
        // Similar to the existing loading grid in the SearchQueryPage XAML
        Device.BeginInvokeOnMainThread(() =>
        {
            // If you have a loading indicator in your ViewModel, update it
            if (BindingContext is SearchViewModel viewModel)
            {
                viewModel.IsLoading = isLoading;
            }
        });
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