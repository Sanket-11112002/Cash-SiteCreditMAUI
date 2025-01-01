using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class LoginPage : ContentPage
{
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnFacebookSignInClicked(object sender, EventArgs e)
    {
    }
    private async void OnGoogleSignInClicked(object sender, EventArgs e)
    { }

}