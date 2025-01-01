using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class PlaceOrderPage : ContentPage
{
    private PlaceOrderViewModel _viewModel;
    public PlaceOrderPage()
    {
        InitializeComponent();
        _viewModel = new PlaceOrderViewModel();
        BindingContext = _viewModel;
    }

    private async void PlaceOrderClick(object sender, EventArgs e)
    {
        await DisplayAlert("Order", "Your order has been placed!", "OK");
    }
}