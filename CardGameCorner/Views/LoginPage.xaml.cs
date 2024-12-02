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

    

}