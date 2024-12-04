//using CardGameCorner.Services;
//using System.ComponentModel;

//namespace CardGameCorner.Views;

//public partial class MyAccountPage : ContentPage
//{
//    private readonly ToolbarItem _editButton;
//    private readonly ToolbarItem _backButton;
//    private readonly ToolbarItem _doneButton;
//    private readonly MyAccountViewModel _viewModel;
//    private readonly IAlertService _alertService;
//    private readonly INavigationService _navigationService;

//    public MyAccountPage(MyAccountViewModel viewModel, IAlertService alertService,INavigationService navigationService)
//    {
//        InitializeComponent();
//        _alertService = alertService;
//        _navigationService = navigationService;
//        BindingContext = _viewModel = viewModel;

//        // Toolbar setup
//        _editButton = new ToolbarItem { Text = "Edit", Command = viewModel.EditCommand };
//        _backButton = new ToolbarItem { Text = "Back", Command = viewModel.BackCommand };
//        _doneButton = new ToolbarItem { Text = "Done", Command = viewModel.DoneCommand };

//        //var logoutButton = new Button
//        //{
//        //    Text = "Logout",
//        //    Command = new Command(async () => await LogoutAsync())
//        //};

//        ToolbarItems.Add(_editButton);
//        viewModel.PropertyChanged += ViewModel_PropertyChanged;
//    }


//    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
//    {
//        if (e.PropertyName == nameof(MyAccountViewModel.IsEditMode))
//        {
//            var viewModel = (MyAccountViewModel)BindingContext;
//            ToolbarItems.Clear();
//            if (viewModel.IsEditMode)
//            {
//                ToolbarItems.Add(_backButton);
//                ToolbarItems.Add(_doneButton);
//            }
//            else
//            {
//                ToolbarItems.Add(_editButton);
//            }
//        }
//    }

//    //protected override async void OnAppearing()
//    //{
//    //    base.OnAppearing();

//    //    // Ensure login before accessing the page
//    //    await EnsureUserLoggedIn();
//    //}

//    //private async Task EnsureUserLoggedIn()
//    //{
//    //    if (!App.IsUserLoggedIn)
//    //    {
//    //        // Clear any existing navigation stack
//    //        await Shell.Current.GoToAsync("//login");

//    //    }
//    //}

//    //protected async override void OnAppearing()
//    //{
//    //    base.OnAppearing();

//    //    // Ensure user is logged in
//    //    if (!App.IsUserLoggedIn)
//    //    {
//    //        // Redirect to login page
//    //        // await Shell.Current.GoToAsync("//login");
//    //        //await Shell.Current.Navigation.PushAsync(new LoginPage());
//    //        await Shell.Current.GoToAsync(nameof(LoginPage));

//    //    }
//    //}

//    protected async override void OnAppearing()
//    {
//        base.OnAppearing();

//        // Check if user is logged in
//        if (!App.IsUserLoggedIn)
//        {
//            bool result = await _alertService.ShowConfirmationAsync(
//                "Login Required",
//                "You need to log in to access this page. Would you like to log in?",
//                "Login",
//                "Continue");

//            if (result)
//            {
//                // User chose to login
//                await _navigationService.NavigateToLoginAsync();
//            }
//            else
//            {
//                // User chose to continue without login
//                // Navigate to home page instead of staying on the current page
//                await _navigationService.NavigateToHomeAsync();
//            }
//        }
//    }

//    private async Task LogoutAsync()
//    {
//        bool confirm = await _alertService.ShowConfirmationAsync(
//            "Logout",
//            "Are you sure you want to log out?");

//        if (confirm)
//        {
//            await _navigationService.LogoutAsync();
//        }
//    }

//    //private async Task LogoutAsync()
//    //{
//    //    bool confirm = await _alertService.ShowConfirmationAsync(
//    //        "Logout",
//    //        "Are you sure you want to log out?");

//    //    if (confirm)
//    //    {
//    //        await _navigationService.LogoutAsync();
//    //    }
//    //}

//    private async void LogoutButton_Clicked(object sender, EventArgs e)
//    {
//        bool confirm = await _alertService.ShowConfirmationAsync(
//            "Logout",
//            "Are you sure you want to log out?");

//        if (confirm)
//        {
//            await _navigationService.LogoutAsync();
//        }
//    }

//}


using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using System.ComponentModel;

namespace CardGameCorner.Views;

public partial class MyAccountPage : ContentPage
{
    private readonly ToolbarItem _editButton;
    private readonly ToolbarItem _backButton;
    private readonly ToolbarItem _doneButton;
    private readonly MyAccountViewModel _viewModel;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;
    private readonly IMyAccountService _myAccountService;
    private readonly Services.ISecureStorage _secureStorage;

    public MyAccountPage(MyAccountViewModel viewModel, IAlertService alertService, INavigationService navigationService)
    {

        InitializeComponent();
        _alertService = alertService;
        _navigationService = navigationService;
        BindingContext = _viewModel = viewModel;

        // Toolbar setup
        _editButton = new ToolbarItem { Text = "Edit", Command = viewModel.EditCommand };
        _backButton = new ToolbarItem { Text = "Back", Command = viewModel.BackCommand };
        _doneButton = new ToolbarItem { Text = "Done", Command = viewModel.DoneCommand };

        // Add initial edit button
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

    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();

    //    // Check if user is logged in
    //    if (!App.IsUserLoggedIn)
    //    {
    //        BindingContext = null;
    //        bool result = await _alertService.ShowConfirmationAsync(
    //            "Login Required",
    //            "You need to log in to access this page. Would you like to log in?",
    //            "Login",
    //            "Continue");

    //        if (result)
    //        {
    //            // User chose to login
    //            await _navigationService.NavigateToLoginAsync();
    //        }
    //        else
    //        {
    //            // User chose to continue without login
    //            await _navigationService.NavigateToHomeAsync();
    //            return;
    //        }
    //    }
    //    else
    //    {
    //        InitializeComponent();

    //       await _viewModel.InitializeAsync();

    //    }
    //    // Initialize the view model and load profile

    //}

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        // Check if user is logged in
        if (!App.IsUserLoggedIn)
        {
            BindingContext = null;
            bool result = await _alertService.ShowConfirmationAsync(
                "Login Required",
                "You need to log in to access this page. Would you like to log in?",
                "Login",
                "Continue");

            if (result)
            {
                // User chose to login
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

            InitializeComponent(); // Reinitialize the page layout
            BindingContext = _viewModel;
            MyAccountViewModel viewmodel = new MyAccountViewModel(_myAccountService, _secureStorage);
            viewmodel.UserProfile = await _viewModel.InitializeAsync(); 
            BindingContext = viewmodel;


        }

        // Initialize the view model and load profile
        await _viewModel.InitializeAsync();
    }

    private async void LogoutButton_Clicked(object sender, EventArgs e)
    {
        bool confirm = await _alertService.ShowConfirmationAsync(
            "Logout",
            "Are you sure you want to log out?");

        if (confirm)
        {
            await _navigationService.LogoutAsync();
        }
    }
}