using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class MyOrdersPage : ContentPage
{
    private readonly MyOrdersViewModel _viewModel;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;
    public MyOrdersPage(MyOrdersViewModel viewModel, IAlertService alertService, INavigationService navigationService)
    {
        _alertService = alertService;
        _navigationService = navigationService;

        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
       
        // Set navigation bar appearance
        Shell.SetNavBarIsVisible(this, true);
        Shell.SetNavBarHasShadow(this, true);
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // Check if user is logged in
        if (!App.IsUserLoggedIn)
        {
            bool result = await _alertService.ShowConfirmationAsync(
                _viewModel.LoginRequiredTitle,
                _viewModel.LoginRequiredMessage,
                _viewModel.LoginText,
                _viewModel.ContinueText);

            if (result)
            {
                // User chose to login
                await SecureStorage.SetAsync("Login", "loginmethod");
                await _navigationService.NavigateToLoginAsync();
            }
            else
            {
                // User chose to continue without login
                await _navigationService.NavigateToHomeAsync();
                return;
            }
        }
        else
        {
            await _viewModel.LoadOrdersAsync();
        }
    }
}