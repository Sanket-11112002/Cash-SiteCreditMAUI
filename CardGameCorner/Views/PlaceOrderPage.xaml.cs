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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.InitializeDropdowns();
    }
}