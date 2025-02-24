using System.Collections.ObjectModel;
using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;
namespace CardGameCorner.Views;

[QueryProperty(nameof(Details), "details")]
public partial class CardDetailPage : ContentPage
{
    private readonly ISecureStorage secureStorage;

    private string _details;
    public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

    public string Details
    {
        get => _details;
        set
        {
            _details = Uri.UnescapeDataString(value);
            var viewModelList = JsonConvert.DeserializeObject<List<CardDetailViewModel>>(_details);
            Console.WriteLine($"Serialized and deserialized successfully: {viewModelList != null}");

            if (viewModelList != null)
            {
                var game = GlobalSettings.SelectedGame;

                foreach (var item in viewModelList)
                {
                    if (item.LanguageConditionsMap != null)
                    {
                        // Populate distinct conditions and languages
                        item.Languages = item.LanguageConditionsMap.Keys.ToList();
                        item.Conditions = item.LanguageConditionsMap.Values
                         .SelectMany(conditions => conditions) // Flatten nested lists
                         .Distinct() // Ensure unique values
                         .ToList();
                        item.siteCredit = item.siteCredit;
                        item.buyList = item.buyList;
                    }
                    else
                    {
                        item.Languages = item.languages.Distinct().ToList();
                        item.Conditions = item.conditions.Distinct().ToList();
                        item.siteCredit = item.siteCredit;
                        item.buyList = item.buyList;
                    }

                    // Set visibility properties
                    item.IsEditionVisibility = game == "pokemon" || game == "yugioh";
                    item.IsReverseVisibility = game == "pokemon";
                    item.IsFoilVisibility = game == "magic";

                    item.InitializeEditMode(item.Id != 0);
                }

                var viewModel = new CardDetailViewModel
                {
                    Cards = new ObservableCollection<CardDetailViewModel>(viewModelList),
                    SelectedCard = viewModelList.FirstOrDefault()
                };

                // Initialize edit mode for the container view model as well
                viewModel.InitializeEditMode(viewModel.SelectedCard?.Id != 0);

                BindingContext = viewModel;

                InitializeComponent();
            }
        }
    }


    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);

    }

    private async void OnAddToMyListClicked(object sender, EventArgs e)
    {
        try
        {
                if (BindingContext is CardDetailViewModel viewModel)
                {
                    var selectedCard = viewModel.SelectedCard;

                    var product = new ProductList
                    {
                        Id = selectedCard.Id,
                        ProductId = selectedCard.ProductId,
                        Game = selectedCard.Game,
                        Model = selectedCard.Name,
                        Image = selectedCard.ImageUrl,
                        Rarity = selectedCard.Rarity,
                        Category = selectedCard.Category,
                        Sitecredit = selectedCard.siteCredit,
                        Buylist = selectedCard.buyList,
                        Quantity = selectedCard.Quantity,
                        Language = selectedCard.SelectedLanguage,
                        Condition = selectedCard.selectedCondition,
                        Languagejsonlst = JsonConvert.SerializeObject(selectedCard.Languages),
                        Conditionjsonlst = JsonConvert.SerializeObject(selectedCard.Conditions),
                       // Username = DecodeJwtAndGetUsername(await SecureStorage.GetAsync("jwt_token")),
                        IsFirstEdition = selectedCard.IsFirstEdition,
                        IsReverse = selectedCard.IsReverse,
                        IsFoil = selectedCard.IsFoil,
                        Evalution = selectedCard.Evalution

                    };

                    var productListService = new SQLiteService();

                    // Add the product to the database
                    await productListService.AddItemToListAsync(product);

                    if (viewModel.IsEditMode)
                    {
                        //await DisplayAlert("Success", "Product Updated to your list!", "OK");
                        await DisplayAlert(
                             viewModel.SuccessAddedToListTitle,
                             viewModel.SuccessUpdateToListMessage,
                             "OK");
                    }
                    else
                    {
                        //await DisplayAlert("Success", "Product added to your list!", "OK");
                        await DisplayAlert(
                            viewModel.SuccessAddedToListTitle,
                            viewModel.SuccessAddedToListMessage,
                            "OK");
                    }
                    viewModel.ExecuteDone();

                    await Shell.Current.Navigation.PopToRootAsync();


                }
                else
                {
                    // await DisplayAlert("Error", "Failed to retrieve product details.", "OK");
                    await DisplayAlert(
                         ((CardDetailViewModel)BindingContext).ErrorRetrieveProductTitle,
                         "Failed to retrieve product details.",
                         "OK");
                }
          //  }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the operation
            // await DisplayAlert("Error", $"Failed to add product: {ex.Message}", "OK");
            await DisplayAlert(
              ((CardDetailViewModel)BindingContext).ErrorAddProductTitle,
              $"Failed to add product: {ex.Message}",
              "OK");
        }
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        Console.WriteLine("CardDetailPage appeared");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Navigate to the SearchPage
       // Shell.Current.GoToAsync($"//{Shell.Current.CurrentItem.CurrentItem.Route}");
    }


    public string DecodeJwtAndGetUsername(string jwtToken)
    {
        try
        {
            // Split the JWT into its three parts
            var parts = jwtToken.Split('.');
            if (parts.Length != 3)
                throw new Exception("Invalid JWT format");

            // Decode the payload (second part of the JWT)
            var payload = parts[1];

            // Base64 decode the payload
            var jsonBytes = Convert.FromBase64String(PadBase64String(payload));
            var jsonString = System.Text.Encoding.UTF8.GetString(jsonBytes);

            // Parse the payload to get the username
            var payloadJson = JObject.Parse(jsonString);

            // Extract the username using the claim name
            var usernameClaim = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name";

            return payloadJson[usernameClaim]?.ToString() ?? throw new Exception("Username claim not found in the JWT");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error decoding JWT: {ex.Message}");
            return null;
        }
    }

    // Helper method to pad Base64 strings properly
    private string PadBase64String(string base64)
    {
        return base64.PadRight(base64.Length + (4 - base64.Length % 4) % 4, '=');
    }

    private void OnIncreaseQuantityClicked(object sender, EventArgs e)
    {
        if (BindingContext is CardDetailViewModel viewModel)
        {
            viewModel.SelectedCard.Quantity++; // Increment the quantity
                                               // SemanticScreenReader.Announce(Counttxt.Text);
        }
    }

    private void OnDecreaseQuantityClicked(object sender, EventArgs e)
    {
        if (BindingContext is CardDetailViewModel viewModel)
        {
            if (viewModel.SelectedCard.Quantity > 1)
            {
                viewModel.SelectedCard.Quantity--; // Decrement the quantity, ensuring it doesn't go below 1
            }
        }
    }


    private void OnPreviousClicked(object sender, EventArgs e)
    {
        if (BindingContext is CardDetailViewModel viewModel)
        {
            var currentIndex = viewModel.Cards.IndexOf(viewModel.SelectedCard);
            if (currentIndex > 0) // Navigate to the previous item if not the first
            {
                viewModel.SelectedCard = viewModel.Cards[currentIndex - 1];
            }
        }
    }

    private void OnNextClicked(object sender, EventArgs e)
    {
        if (BindingContext is CardDetailViewModel viewModel)
        {
            var currentIndex = viewModel.Cards.IndexOf(viewModel.SelectedCard);
            if (currentIndex < viewModel.Cards.Count - 1) // Navigate to the next item if not the last
            {
                viewModel.SelectedCard = viewModel.Cards[currentIndex + 1];
            }
        }
    }
   

}