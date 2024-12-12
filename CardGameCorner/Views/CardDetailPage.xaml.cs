


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

        public string Details
        {
            get => _details;
            set
            {
                _details = Uri.UnescapeDataString(value);
                var viewModelList = JsonConvert.DeserializeObject<List<CardDetailViewModel>>(_details);
                  

                if (viewModelList != null)
                {

                    // Check for conditions and update UI
                    foreach (var item in viewModelList)
                    {
                    item.Languages = item.Languages?.Distinct().ToList();
                    item.Conditions = item.Conditions?.Distinct().ToList();
                    if (item.Id != 0)
                        {
                            BtnText.Text = "Update to my list";
                            break;
                        }
                    }

                    // Bind the ViewModel as required
                    BindingContext = new CardDetailViewModel
                    {

                        Cards = new ObservableCollection<CardDetailViewModel>(viewModelList),
                        SelectedCard = viewModelList.FirstOrDefault()
                    };
                }
            }
        }

        public CardDetailPage()
        {
            InitializeComponent();
        }
       
        
    //public CardDetailPage()
    //{

    //    //InitializeComponent();
    //    InitializeComponent();
    //    var detailsJson = this.GetQueryParameter("details");
    //    if (!string.IsNullOrEmpty(detailsJson))
    //    {
    //        var lst = JsonConvert.DeserializeObject<List<CardDetailViewModel>>(detailsJson);
    //        // Use 'lst' as needed
    //    }
    //    //BindingContext = viewmodel;

    //    foreach (var i in viewmodel)
    //    {
    //        if (i.Id != 0 && i.Id != null)
    //        {
    //            BtnText.Text = "Update to my list";
    //        }

    //    }
    //    // Bind a wrapping ViewModel containing the card collection
    //    BindingContext = new CardDetailViewModel
    //    {
    //        Cards = new ObservableCollection<CardDetailViewModel>(viewmodel),
    //        SelectedCard = viewmodel.FirstOrDefault() // Default to the first card
    //    };

    //    try
    //    {
    //        var navigation = Shell.Current?.Navigation;
    //        if (navigation != null && navigation.NavigationStack != null)
    //        {
    //            var stack = navigation.NavigationStack;
    //            foreach (var page in stack)
    //            {
    //                Console.WriteLine(page.GetType().Name);
    //            }
    //        }
    //        else
    //        {
    //            Console.WriteLine("Navigation or NavigationStack is null.");
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Console.WriteLine($"Error logging navigation stack: {e.Message}");
    //    }

    //}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
        
    }


    private async void OnAddToMyListClicked(object sender, EventArgs e)
    {
        try
        {
            if (!App.IsUserLoggedIn)
            {
                var _alertService = new AlertService();

                var _navigationService = new NavigationService(_alertService, secureStorage);
                //bool result = await _alertService.ShowConfirmationAsync(
                // "Login Required",
                // "You need to log in to add card to list. Would you like to log in?",
                // "Login",
                // "Continue");

                bool result = await _alertService.ShowConfirmationAsync(
                  ((CardDetailViewModel)BindingContext).LoginRequiredTitle,
                  ((CardDetailViewModel)BindingContext).LoginRequiredMessage,
                  ((CardDetailViewModel)BindingContext).LoginText,
                  ((CardDetailViewModel)BindingContext).ContinueText);

                if (result)
                {
                    // User chose to login
                    await _navigationService.NavigateToLoginAsync();
                }
                else
                {
                    string detailsJson = await SecureStorage.GetAsync("CardDetailsObject");
                    var details = JsonConvert.DeserializeObject<CardDetailViewModel>(detailsJson);


                    var detaillst = new List<CardDetailViewModel>();
                    detaillst.Add(details);
                    // User chose to continue without login
                     // await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(detaillst));
                    return;
                }
            }
            else
            {
                // User is logged in
                if (BindingContext is CardDetailViewModel viewModel)
                {
                    // Retrieve JWT token from secure storage
                    var jwtToken = await SecureStorage.GetAsync("jwt_token");
                    if (string.IsNullOrEmpty(jwtToken))
                    {
                        // await DisplayAlert("Error", "Failed to retrieve authentication token.", "OK");
                        await DisplayAlert(
                           viewModel.ErrorRetrieveTokenTitle,
                           viewModel.ErrorRetrieveTokenMessage,
                           "OK");
                        return;
                    }

                    // Decode and extract username from the token
                    var username = DecodeJwtAndGetUsername(jwtToken);

                    var selectedCard = viewModel.SelectedCard;

                    var product = new ProductList
                    {
                        Id = selectedCard.Id,
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
                        Username = DecodeJwtAndGetUsername(await SecureStorage.GetAsync("jwt_token")),
                        IsFirstEdition = selectedCard.IsFirstEdition,
                        IsReverse = selectedCard.IsReverse
                    };

                    var productListService = new SQLiteService();

                    // Add the product to the database
                    await productListService.AddItemToListAsync(product);

                    if (viewModel != null && viewModel.Id != 0)
                    {
                        //await DisplayAlert("Success", "Product Updated to your list!", "OK");
                        await DisplayAlert(
                             viewModel.SuccessAddedToListTitle,
                             viewModel.SuccessAddedToListMessage,
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

                    await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

                  
                }
                else
                {
                    // await DisplayAlert("Error", "Failed to retrieve product details.", "OK");
                    await DisplayAlert(
                         ((CardDetailViewModel)BindingContext).ErrorRetrieveProductTitle,
                         "Failed to retrieve product details.",
                         "OK");
                }
            }
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
            if (viewModel.Quantity > 1)
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