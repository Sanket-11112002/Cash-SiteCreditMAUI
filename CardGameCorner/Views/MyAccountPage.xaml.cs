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

// Last Working Code Before Lang Translation


//using CardGameCorner.Services;
//using CardGameCorner.ViewModels;
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
//    private readonly IMyAccountService _myAccountService;
//    private readonly Services.ISecureStorage _secureStorage;

//    public MyAccountPage(MyAccountViewModel viewModel, IAlertService alertService, INavigationService navigationService)
//    {

//        InitializeComponent();
//        _alertService = alertService;
//        _navigationService = navigationService;
//        BindingContext = _viewModel = viewModel;

//        // Toolbar setup
//        _editButton = new ToolbarItem { Text = "Edit", Command = viewModel.EditCommand };
//        _backButton = new ToolbarItem { Text = "Back", Command = viewModel.BackCommand };
//        _doneButton = new ToolbarItem { Text = "Done", Command = viewModel.DoneCommand };

//        // Add initial edit button
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

//    //protected async override void OnAppearing()
//    //{
//    //    base.OnAppearing();

//    //    // Check if user is logged in
//    //    if (!App.IsUserLoggedIn)
//    //    {
//    //        BindingContext = null;
//    //        bool result = await _alertService.ShowConfirmationAsync(
//    //            "Login Required",
//    //            "You need to log in to access this page. Would you like to log in?",
//    //            "Login",
//    //            "Continue");

//    //        if (result)
//    //        {
//    //            // User chose to login
//    //            await _navigationService.NavigateToLoginAsync();
//    //        }
//    //        else
//    //        {
//    //            // User chose to continue without login
//    //            await _navigationService.NavigateToHomeAsync();
//    //            return;
//    //        }
//    //    }
//    //    else
//    //    {
//    //        InitializeComponent();

//    //       await _viewModel.InitializeAsync();

//    //    }
//    //    // Initialize the view model and load profile

//    //}

//    protected async override void OnAppearing()
//    {
//        base.OnAppearing();

//        // Check if user is logged in
//        if (!App.IsUserLoggedIn)
//        {
//            BindingContext = null;
//            bool result = await _alertService.ShowConfirmationAsync(
//                "Login Required",
//                "You need to log in to access this page. Would you like to log in?",
//                "Login",
//                "Continue");

//            if (result)
//            {
//                // User chose to login
//                await SecureStorage.SetAsync("Login", "loginmethod");
//                await _navigationService.NavigateToLoginAsync();
//            }
//            else
//            {
//                // User chose to continue without login
//                await _navigationService.NavigateToHomeAsync();
//                return;
//            }
//        }
//        else
//        {

//            InitializeComponent(); // Reinitialize the page layout
//            BindingContext = _viewModel;
//            MyAccountViewModel viewmodel = new MyAccountViewModel(_myAccountService, _secureStorage);
//            viewmodel.UserProfile = await _viewModel.InitializeAsync(); 
//            BindingContext = viewmodel;
//        }
//        // Initialize the view model and load profile
//        await _viewModel.InitializeAsync();
//    }
//}

using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using System.ComponentModel;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;
namespace CardGameCorner.Views;

public partial class MyAccountPage : ContentPage
{
    private ToolbarItem _editButton;
    private ToolbarItem _backButton;
    private ToolbarItem _doneButton;
    private readonly MyAccountViewModel _viewModel;
    private readonly IAlertService _alertService;
    private readonly INavigationService _navigationService;
    private readonly IMyAccountService _myAccountService;
    private readonly ISecureStorage _secureStorage;

    public MyAccountPage(MyAccountViewModel viewModel, IAlertService alertService, INavigationService navigationService,
                         IMyAccountService myAccountService, ISecureStorage secureStorage)
    {
        _alertService = alertService;
        _navigationService = navigationService;
        _myAccountService = myAccountService;
        _secureStorage = secureStorage;

        _viewModel = viewModel;
        BindingContext = _viewModel;

        InitializeComponent();
        // Initialize toolbar items with initial text
        InitializeToolbarItems();

        // Subscribe to property changes
        viewModel.PropertyChanged += ViewModel_PropertyChanged;

       // InitializeComponent();
    }

    private void InitializeToolbarItems()
    {
        // Create toolbar items with initial text from ViewModel
        _editButton = new ToolbarItem
        {
            Text = _viewModel.EditButtonText,
            Command = _viewModel.EditCommand
        };

        _backButton = new ToolbarItem
        {
            Text = _viewModel.BackButtonText,
            Command = _viewModel.BackCommand
        };

        _doneButton = new ToolbarItem
        {
            Text = _viewModel.DoneButtonText,
            Command = _viewModel.DoneCommand
        };

        // Add initial edit button
        ToolbarItems.Add(_editButton);
    }

    private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MyAccountViewModel.IsEditMode) ||
            e.PropertyName == nameof(MyAccountViewModel.EditButtonText) ||
            e.PropertyName == nameof(MyAccountViewModel.BackButtonText) ||
            e.PropertyName == nameof(MyAccountViewModel.DoneButtonText))
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                // Update toolbar items with current ViewModel text
                _editButton.Text = _viewModel.EditButtonText;
                _backButton.Text = _viewModel.BackButtonText;
                _doneButton.Text = _viewModel.DoneButtonText;

                ToolbarItems.Clear();
                if (_viewModel.IsEditMode)
                {
                    ToolbarItems.Add(_backButton);
                    ToolbarItems.Add(_doneButton);
                }
                else
                {
                    ToolbarItems.Add(_editButton);
                }
            });
        }
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        // Check if user is logged in
        if (!App.IsUserLoggedIn)
        {
            _viewModel.UserProfile = null;
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
            // Ensure the ViewModel is properly initialized with user details
            var data = await _viewModel.InitializeAsync();
            _viewModel.UserProfile = data;
           // InitializeComponent();
            //BindingContext = _viewModel;
        }
    }

    protected override bool OnBackButtonPressed()
    {

        Task<bool> answer = DisplayAlert(AppResources.Exit, AppResources.ExitApp, AppResources.YesMsg, "No");
        answer.ContinueWith(task =>
        {
            if (task.Result)
            {
                Application.Current.Quit();
            }
        });
        return true;
    }
}