using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class MyOrdersPage : ContentPage
{
    private readonly MyOrdersViewModel _viewModel;

    public MyOrdersPage(MyOrdersViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;

        // Set navigation bar appearance
        Shell.SetNavBarIsVisible(this, true);
        Shell.SetNavBarHasShadow(this, true);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.LoadOrdersAsync();
    }
}