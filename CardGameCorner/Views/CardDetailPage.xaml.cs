


using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;


namespace CardGameCorner.Views;

public partial class CardDetailPage : ContentPage
{
    private readonly ISecureStorage secureStorage;

    int count = 0;
    public CardDetailPage(CardDetailViewModel viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
      


        if (viewmodel.Id!=0 && viewmodel.Id!=null) {
           // BtnText.Text = "Update to my list";
        }

       

    }

    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //     Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

    //     Shell.Current.GoToAsync("//SearchPage");
    //}




    //private async void OnAddToMyListClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        // Fetch data from the BindingContext (ViewModel)
    //        if (BindingContext is CardDetailViewModel viewModel)
    //        {
    //            // Example data fetched from the ViewModel
    //            var product = new ProductList
    //            {
    //                Game = viewModel.Game,               // Replace with the actual property from ViewModel
    //                Model = viewModel.Name,             // Replace with the actual property from ViewModel
    //                Image = viewModel.imageUrl,         // Replace with the actual property from ViewModel
    //                Rarity = viewModel.Rarity,          // Replace with the actual property from ViewModel
    //                Category = viewModel.Category,      // Replace with the actual property from ViewModel
    //                Sitecredit = viewModel.siteCredit,  // Replace with the actual property from ViewModel
    //                Buylist = viewModel.Buylist,        // Replace with the actual property from ViewModel
    //                Quantity = viewModel.Quantity,
    //                Language = viewModel.selectedLanguage,
    //                Condition = viewModel.selectedCondition,
    //                Languagejsonlst = JsonConvert.SerializeObject(viewModel.languages),
    //                Conditionjsonlst = JsonConvert.SerializeObject(viewModel.conditions),
    //                IsFirstEdition = false,
    //                IsReverse = true
    //            };

    //            // Initialize the SQLite service
    //            var productListService = new SQLiteService();

    //            // Add the product to the database
    //            await productListService.AddItemToListAsync(product);

    //            // Display a success message
    //            await DisplayAlert("Success", "Product added to your list!", "OK");
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", "Failed to retrieve product details.", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle any exceptions that occur during the operation
    //        await DisplayAlert("Error", $"Failed to add product: {ex.Message}", "OK");
    //    }
    //}


    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //    count++;

    //    if (count == 1)
    //        CounterBtn.Text = $"Clicked {count} time";
    //    else
    //        CounterBtn.Text = $"Clicked {count} times";

    //    SemanticScreenReader.Announce(CounterBtn.Text);
    //}
    //private async void OnAddToMyListClicked(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (!App.IsUserLoggedIn)
    //        {
    //            var _alertService = new AlertService();

    //            var _navigationService = new NavigationService(_alertService, secureStorage);
    //            bool result = await _alertService.ShowConfirmationAsync(
    //             "Login Required",
    //             "You need to log in to add card to list. Would you like to log in?",
    //             "Login",
    //             "Continue");

    //            if (result)
    //            {
    //                // User chose to login

    //                await _navigationService.NavigateToLoginAsync();


    //            }
    //            else
    //            {
    //                string detailsJson = await SecureStorage.GetAsync("CardDetailsObject");

    //                var details = JsonConvert.DeserializeObject<CardDetailViewModel>(detailsJson);

    //                // User chose to continue without login
    //                await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));
    //                return;
    //            }

    //        }
    //        else
    //        {


    //            if (BindingContext is CardDetailViewModel viewModel)
    //            {


    //                var product = new ProductList
    //                {
    //                    Id = viewModel.Id,
    //                    Game = viewModel.Game,
    //                    Model = viewModel.Name,
    //                    Image = viewModel.imageUrl,
    //                    Rarity = viewModel.Rarity,
    //                    Category = viewModel.Category,
    //                    Sitecredit = viewModel.siteCredit,
    //                    Buylist = viewModel.Buylist,
    //                    Quantity = viewModel.Quantity,
    //                    Language = viewModel.selectedLanguage,
    //                    Condition = viewModel.selectedCondition,
    //                    Languagejsonlst = JsonConvert.SerializeObject(viewModel.languages),
    //                    Conditionjsonlst = JsonConvert.SerializeObject(viewModel.conditions), // This will serialize the list to JSON
    //                    IsFirstEdition = false,
    //                    IsReverse = true
    //                };
    //                var productListService = new SQLiteService();

    //                // Add the product to the database
    //                await productListService.AddItemToListAsync(product);

    //                if (viewModel != null && viewModel.Id != 0)
    //                {
    //                    await DisplayAlert("Success", "Product Updated to your list!", "OK");
    //                }
    //                else
    //                {
    //                    await DisplayAlert("Success", "Product added to your list!", "OK");
    //                }
    //                viewModel.ExecuteDone();
    //            }

    //            // Example data fetched from the ViewModel

    //            // Initialize the SQLite service

    //            // Display a success message


    //            else
    //            {
    //                await DisplayAlert("Error", "Failed to retrieve product details.", "OK");
    //            }

    //            // Fetch data from the BindingContext (ViewModel)

    //        }


    //    }
    //    catch (Exception ex)
    //    {
    //        // Handle any exceptions that occur during the operation
    //        await DisplayAlert("Error", $"Failed to add product: {ex.Message}", "OK");
    //    }
    //}

    private async void OnAddToMyListClicked(object sender, EventArgs e)
    {
        try
        {
            if (!App.IsUserLoggedIn)
            {
                var _alertService = new AlertService();

                var _navigationService = new NavigationService(_alertService, secureStorage);
                bool result = await _alertService.ShowConfirmationAsync(
                 "Login Required",
                 "You need to log in to add card to list. Would you like to log in?",
                 "Login",
                 "Continue");

                if (result)
                {
                    // User chose to login
                    await _navigationService.NavigateToLoginAsync();
                }
                else
                {
                    string detailsJson = await SecureStorage.GetAsync("CardDetailsObject");
                    var details = JsonConvert.DeserializeObject<CardDetailViewModel>(detailsJson);

                    // User chose to continue without login
                    await Application.Current.MainPage.Navigation.PushAsync(new CardDetailPage(details));
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
                        await DisplayAlert("Error", "Failed to retrieve authentication token.", "OK");
                        return;
                    }

                    // Decode and extract username from the token
                    var username = DecodeJwtAndGetUsername(jwtToken);

                    var product = new ProductList
                    {
                        Id = viewModel.Id,
                        Game = viewModel.Game,
                        Model = viewModel.Name,
                        Image = viewModel.imageUrl,
                        Rarity = viewModel.Rarity,
                        Category = viewModel.Category,
                        Sitecredit = viewModel.siteCredit,
                        Buylist = viewModel.Buylist,
                        Quantity = viewModel.Quantity,
                        Language = viewModel.selectedLanguage,
                        Condition = viewModel.selectedCondition,
                        Languagejsonlst = JsonConvert.SerializeObject(viewModel.languages),
                        Conditionjsonlst = JsonConvert.SerializeObject(viewModel.conditions), // Serialize the list to JSON
                        Username = username, // Save the username from the JWT token
                        IsFirstEdition = false,
                        IsReverse = true
                    };

                    var productListService = new SQLiteService();

                    // Add the product to the database
                    await productListService.AddItemToListAsync(product);

                    if (viewModel != null && viewModel.Id != 0)
                    {
                        await DisplayAlert("Success", "Product Updated to your list!", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Success", "Product added to your list!", "OK");
                    }
                    viewModel.ExecuteDone();

                    await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

                  
                }
                else
                {
                    await DisplayAlert("Error", "Failed to retrieve product details.", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the operation
            await DisplayAlert("Error", $"Failed to add product: {ex.Message}", "OK");
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
            viewModel.Quantity++; // Increment the quantity
           // SemanticScreenReader.Announce(Counttxt.Text);
        }
    }

    private void OnDecreaseQuantityClicked(object sender, EventArgs e)
    {
        if (BindingContext is CardDetailViewModel viewModel)
        {
            if (viewModel.Quantity > 1)
            {
                viewModel.Quantity--; // Decrement the quantity, ensuring it doesn't go below 1
            }
        }
    }
  

}