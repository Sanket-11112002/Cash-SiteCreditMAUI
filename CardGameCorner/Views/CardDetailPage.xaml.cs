

using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;


namespace CardGameCorner.Views;

public partial class CardDetailPage : ContentPage
{
    int count = 0;
    public CardDetailPage(CardDetailViewModel viewmodel)
    {
        InitializeComponent();
        BindingContext = viewmodel;
      
    }

    private async void OnAddToMyListClicked(object sender, EventArgs e)
    {
        try
        {
            // Fetch data from the BindingContext (ViewModel)
            if (BindingContext is CardDetailViewModel viewModel)
            {
                // Example data fetched from the ViewModel
                var product = new ProductList
                {
                    Game = viewModel.Game,               // Replace with the actual property from ViewModel
                    Model = viewModel.Name,             // Replace with the actual property from ViewModel
                   // ModelEn = viewModel.ModelEn,         // Replace with the actual property from ViewModel
                    Image = viewModel.imageUrl,             // Replace with the actual property from ViewModel
                    //Color = viewModel.Color,             // Replace with the actual property from ViewModel
                    Rarity = viewModel.Rarity,           // Replace with the actual property from ViewModel
                    Category = viewModel.Category,       // Replace with the actual property from ViewModel
                    Sitecredit = viewModel.siteCredit,       // Replace with the actual property from ViewModel
                    Buylist = viewModel.Buylist,       // Replace with the actual property from ViewModel
                    Quantity = viewModel.Quantity  ,
                    Language=viewModel.selectedLanguage,
                    Condition=viewModel.selectedCondition
                    // Replace with the actual property from ViewModel
                };

                // Initialize the SQLite service
                var productListService = new SQLiteService();

                // Add the product to the database
                await productListService.AddItemToListAsync(product);

                // Display a success message
                await DisplayAlert("Success", "Product added to your list!", "OK");
            }
            else
            {
                await DisplayAlert("Error", "Failed to retrieve product details.", "OK");
            }
        }
        catch (Exception ex)
        {
            // Handle any exceptions that occur during the operation
            await DisplayAlert("Error", $"Failed to add product: {ex.Message}", "OK");
        }
    }

    //private void OnCounterClicked(object sender, EventArgs e)
    //{
    //    count++;

    //    if (count == 1)
    //        CounterBtn.Text = $"Clicked {count} time";
    //    else
    //        CounterBtn.Text = $"Clicked {count} times";

    //    SemanticScreenReader.Announce(CounterBtn.Text);
    //}

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