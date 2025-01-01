using System.Text.Json;
using CardGameCorner.Models;
using CardGameCorner.ViewModels;
using CardGameCorner.Services;
using Microsoft.Maui.Controls;
using Newtonsoft.Json;
using Microsoft.VisualStudio.Utilities;
using Microsoft.Maui.Storage;


namespace CardGameCorner.Views;

public partial class SearchQueryPage : ContentPage
{
    private readonly IScanCardService _service;
    public SearchQueryPage()
    {
        InitializeComponent();

    }

    protected override async void OnAppearing()
    {
        var cards = new SearchViewModel();

        InitializeComponent();
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        // Access the ViewModel
        var viewModel = BindingContext as SearchViewModel;
        if (viewModel == null)
            return;

        // Execute the SearchCommand if available
        if (viewModel.SearchCommand.CanExecute(null))
        {
            viewModel.SearchCommand.Execute(null);
        }
    }
    private async void OnUploadButtonClicked(object sender, EventArgs e)
    {
        // Show loading overlay
        SetLoadingState(true);
        var response = new ListBoxService();
        var conditinlst = await response.GetConditionsAsync();
        var lnglst = await response.GetLanguagesAsync();

        try
        {
            var viewModel = BindingContext as SearchViewModel;
            if (viewModel == null)
            {
                await DisplayAlert("Error", "ViewModel is not set.", "OK");
                SetLoadingState(false);
                return;
            }
            var imageButton = sender as ImageButton;
            if (imageButton == null)
            {

                await DisplayAlert("Error", "The ImageButton is null.", "OK");
                return;
            }

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
                SetLoadingState(false);
                return;
            }

            // Download the image from the URL
            var imageBytes = await DownloadImageAsync(imageUrl);
            if (imageBytes == null || imageBytes.Length == 0)
            {
                await DisplayAlert("Error", "Failed to download image.", "OK");
                SetLoadingState(false);
                return;
            }

            Console.WriteLine("Image downloaded successfully!");

            var compressedImageStream = await viewModel.CompressImageAsync(new MemoryStream(imageBytes), 100 * 1024);


            var uploadStream = new MemoryStream();
            compressedImageStream.Position = 0;
            await compressedImageStream.CopyToAsync(uploadStream);
            uploadStream.Position = 0;

            // Upload the image
            //var apiResponse = await viewModel.UploadImageAsync(uploadStream);


            //Console.WriteLine($"Upload successful: {apiResponse}");

            var cardRequest = new CardSearchRequest
            {
                Title = selectedCard.Model,
                Set = selectedCard.SetCode,
                Game = selectedCard.Game,
                Lang = "en",
                Foil = 0,
                FirstEdition = 0
            };

            // Search for the card based on the uploaded image
            CardComparisonViewModel data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(uploadStream.ToArray())));

            if (data != null && data.responseContent.Products != null)
            {
                var detailslst = new List<CardDetailViewModel>();
                foreach (var product in data.responseContent.Products)
                {
                    // Deserialize the Variants JSON string into a list of ProductVariant1 objects
                    var variants = product.Variants;

                    if (variants != null)
                    {

                        var languageConditionsMap = variants
                      .GroupBy(v => v.Language) // Group variants by Language
                      .ToDictionary(
                          group => group.Key,                     // Language as key
                          group => group.Select(v => v.Condition) // Select Conditions for each language
                                       .Distinct()               // Get distinct conditions
                                       .ToList()                 // Convert to List
                      );
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
                            // id= int.Parse(product.Id),
                            Languages = distinctLanguages,
                            Conditions = distinctConditions,
                            Name = product.Model,
                            Rarity = product.Rarity,
                            Category = product.Category,
                            ImageUrl = "https://www.cardgamecorner.com" + product.Image,
                            game = product.Game,
                            LanguageConditionsMap = languageConditionsMap,
                            //IsFirstEdition=product.Variants.Any(v => !string.IsNullOrEmpty(v.FirstEdition)), // Check if any variant is marked as FirstEdition
                            SelectedLanguage = lnglst.Where(item => item.Id == product.Language).Select(item => item.Language).FirstOrDefault(),
                            selectedCondition = conditinlst.Where(item => item.Id == product.Condition).Select(item => item.Condition).FirstOrDefault(),
                            varinats = variants.ToList()


                        };
                        detailslst.Add(details);


                        // Navigate to the CardDetailsPage for each product

                    }

                    else
                    {
                        Console.WriteLine("No variants found for product.");
                    }
                }



                // await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(detailslst));
                // Serialize the details list to a string
                var detailsJson = JsonConvert.SerializeObject(detailslst);  // Ensure you have 'Newtonsoft.Json' or other serializer for this


                // Navigate using GoToAsync with the serialized data as a query parameter
                await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailsJson)}");

                //  await Shell.Current.GoToAsync($"{nameof(CardDetailPage)}?details={Uri.EscapeDataString(detailslst)}");
                //  viewModel.OnUploadButtonClicked(detailslst);
            }
            else
            {
                await DisplayAlert("Error", "No products found.", "OK");
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
        DisplayAlert("Card", "favouritee", "OK");

    }
}
