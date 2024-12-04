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

            // Clone the compressed stream for display
            var displayStream = new MemoryStream();
            compressedImageStream.Position = 0;
            await compressedImageStream.CopyToAsync(displayStream);
            displayStream.Position = 0;

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
                CardComparisonViewModel data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

                if (data != null && data.responseContent.Products.Count > 0)
                {
                    var product = data.responseContent.Products[0];
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

                        // Dismiss loading before navigation
                        SetLoadingState(false);

                        // Navigate to the CardDetailsPage
                        await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));
                    }
                    else
                    {
                        SetLoadingState(false);
                        await DisplayAlert("Error", "Something went wrong", "OK");
                    }
                }
                else
                {
                    SetLoadingState(false);
                    await DisplayAlert("Error", "Card not found", "OK");
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