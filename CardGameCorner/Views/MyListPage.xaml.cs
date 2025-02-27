////using CardGameCorner.Services;
////using CardGameCorner.ViewModels;

////namespace CardGameCorner.Views
////{
////    public partial class MyListPage : ContentPage
////    {

////        private readonly MyListViewModel _viewModel;


////        public MyListPage()
////        {
////            InitializeComponent();
////            _viewModel = new MyListViewModel();

////            BindingContext = _viewModel;


////        }



////        protected async override void OnAppearing()
////        {
////            base.OnAppearing();

////            try
////            {
////                // Refresh data when the page appears
////                await _viewModel.LoadDataAsync();



////                // Optionally update UI elements or display messages
////                if (_viewModel.CardItems == null || !_viewModel.CardItems.Any())
////                {
////                    await DisplayAlert("Info", "Your list is empty. Add items to see them here.", "OK");
////                }


////            }
////            catch (Exception ex)
////            {
////                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
////            }
////        }
////    }


////    //protected async override void OnAppearing()
////    //{
////    //    base.OnAppearing();


////    //    if (GlobalSettings.SelectedGame != null)
////    //    {
////    //        InitializeComponent();

////    //    }
////    //    else
////    //    {



////    //        await Application.Current.MainPage.DisplayAlert("Error", "No game selected. Please select a game before accessing the search page.", "OK");
////    //        await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

////    //        await Shell.Current.GoToAsync("//home");

////    //    }
////    //}


////}


//using CardGameCorner.Services;
//using CardGameCorner.ViewModels;

//namespace CardGameCorner.Views
//{
//    public partial class MyListPage : ContentPage
//    {
//        private readonly MyListViewModel _viewModel;

//        public MyListPage()
//        {
//            InitializeComponent();
//            _viewModel = new MyListViewModel();
//            BindingContext = _viewModel;
//        }

//        protected async override void OnAppearing()
//        {
//            base.OnAppearing();

//            // Check if user is logged in
//            if (!App.IsUserLoggedIn)
//            {
//                // Prompt user to login or continue
//                bool result = await DisplayAlert(
//                    "Login Required",
//                    "You need to log in to access this page. Would you like to log in?",
//                    "Login",
//                    "Continue");

//                if (result)
//                {
//                    // User chose to login
//                    await Shell.Current.GoToAsync(nameof(LoginPage));
//                }
//                else
//                {
//                    // User chose to continue without login
//                    // Navigate back to previous page or home
//                    await Shell.Current.GoToAsync("..");
//                    return;
//                }
//            }

//            try
//            {
//                // Refresh data when the page appears
//                await _viewModel.LoadDataAsync();

//                // Optionally update UI elements or display messages
//                if (_viewModel.CardItems == null || !_viewModel.CardItems.Any())
//                {
//                    await DisplayAlert("Info", "Your list is empty. Add items to see them here.", "OK");
//                }
//            }
//            catch (Exception ex)
//            {
//                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
//            }
//        }
//    }
//}

using System.Diagnostics;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views
{
    public partial class MyListPage : ContentPage
    {
        private readonly MyListViewModel _viewModel;
        private readonly IAlertService _alertService;
        private readonly INavigationService _navigationService;

        public MyListPage(
            MyListViewModel viewModel,
            IAlertService alertService,
            INavigationService navigationService)
        {
            InitializeComponent();

            _viewModel = viewModel;
            _alertService = alertService;
            _navigationService = navigationService;

            BindingContext = _viewModel;

            // Add logout button
            var logoutButton = new Button
            {
                Text = "Logout",
                Command = new Command(async () => await LogoutAsync())
            };

            // Add the logout button to your layout
            // Assuming you have a StackLayout or similar in your XAML
            // mainLayout.Children.Add(logoutButton);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();

            // Check if user is logged in
            //if (!App.IsUserLoggedIn)
            //{
            //     //_viewModel.CardItems = null;
            //    bool result = await _alertService.ShowConfirmationAsync(
            //         _viewModel.LoginRequiredTitle,
            //         _viewModel.LoginRequiredMessage,
            //         _viewModel.LoginText,
            //         _viewModel.ContinueText);

            //    if (result)
            //    {
            //        // User chose to login
            //        await _navigationService.NavigateToLoginAsync();
            //    }
            //    else
            //    {
            //        // User chose to continue without login
            //        // Navigate to home page instead of staying on the current page
            //        await _navigationService.NavigateToHomeAsync();
            //    }
            //}
            //else
            //{
            // Load data only if logged in

            try
            {

                await _viewModel.LoadDataAsync();

                if (_viewModel.CardItems == null || !_viewModel.CardItems.Any())
                {
                    //await _alertService.ShowAlertAsync(
                    // AppResources.EmptyListTitle,
                    // AppResources.EmptyListMessage);

                    _viewModel.listvisibility = true;
                    _viewModel.listvisibilitycards = false;
                }
                else
                {
                    _viewModel.listvisibilitycards = true;
                    _viewModel.listvisibility = false;
                }
            }
            catch (Exception ex)
            {
                await _alertService.ShowAlertAsync(
               AppResources.ErrorTitle,
               string.Format(AppResources.DataLoadErrorMessage, ex.Message));
            }
            // }
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
        private async Task LogoutAsync()
        {
            bool confirm = await _alertService.ShowConfirmationAsync(
                "Logout",
                "Are you sure you want to log out?");

            if (confirm)
            {
                this.BindingContext = null;
                await _navigationService.LogoutAsync();
            }
        }

        //private async void PlaceOrder_Clicked(object sender, EventArgs e)
        //{
        //    await Shell.Current.GoToAsync("PlaceOrderPage");
        //    // await Navigation.PushAsync(new ScanPage());
        //}

        private async void PlaceOrder_Clicked(object sender, EventArgs e)
        {
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
                    await _navigationService.NavigateToLoginAsync();
                }
                //else
                //{
                //    // User chose to continue without login
                //    // Navigate to home page instead of staying on the current page
                //    await _navigationService.NavigateToHomeAsync();
                //}
            }
            else
            {
                // User is logged in, proceed to place order
                await Shell.Current.GoToAsync("PlaceOrderPage");
            }
        }
    }
}