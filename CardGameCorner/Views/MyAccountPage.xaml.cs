using CardGameCorner.Services;
using System.ComponentModel;

namespace CardGameCorner.Views;

public partial class MyAccountPage : ContentPage
{
    private readonly ToolbarItem _editButton;
    private readonly ToolbarItem _backButton;
    private readonly ToolbarItem _doneButton;

    public MyAccountPage(MyAccountViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;

        // Toolbar setup
        _editButton = new ToolbarItem { Text = "Edit", Command = viewModel.EditCommand };
        _backButton = new ToolbarItem { Text = "Back", Command = viewModel.BackCommand };
        _doneButton = new ToolbarItem { Text = "Done", Command = viewModel.DoneCommand };

        ToolbarItems.Add(_editButton);
        viewModel.PropertyChanged += ViewModel_PropertyChanged;
    }


    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MyAccountViewModel.IsEditMode))
        {
            var viewModel = (MyAccountViewModel)BindingContext;
            ToolbarItems.Clear();
            if (viewModel.IsEditMode)
            {
                ToolbarItems.Add(_backButton);
                ToolbarItems.Add(_doneButton);
            }
            else
            {
                ToolbarItems.Add(_editButton);
            }
        }
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Ensure login before accessing the page
    //    await EnsureUserLoggedIn();
    //}

    //private async Task EnsureUserLoggedIn()
    //{
    //    if (!App.IsUserLoggedIn)
    //    {
    //        // Clear any existing navigation stack
    //        await Shell.Current.GoToAsync("//login");

    //    }
    //}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Ensure user is logged in
        if (!App.IsUserLoggedIn)
        {
            // Redirect to login page
            // await Shell.Current.GoToAsync("//login");
            //await Shell.Current.Navigation.PushAsync(new LoginPage());
            await Shell.Current.GoToAsync(nameof(LoginPage));

        }
    }

    private async Task LogoutAsync()
    {
        App.IsUserLoggedIn = false;
        await Shell.Current.GoToAsync("//login");
    }

  

}
