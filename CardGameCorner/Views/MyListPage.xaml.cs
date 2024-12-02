using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views
{
    public partial class MyListPage : ContentPage
    {

        private readonly MyListViewModel _viewModel;


        public MyListPage()
        {
            InitializeComponent();
            _viewModel = new MyListViewModel();
         
            BindingContext = _viewModel;
            

        }
        private async void OnSettingsClicked(object sender, EventArgs e)
        {
            // Use the global settings service to show settings
            var globalSettings = GlobalSettingsService.Current;

            string result = await DisplayActionSheet(
                "Settings",
                "Cancel",
                null,
                "Select Language",
                "Select Game");

            switch (result)
            {
                case "Select Language":
                    await globalSettings.ChangeLanguageAsync();
                    break;
                case "Select Game":
                    await globalSettings.ChangeGameAsync();
                    break;
            }
        }


        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                // Refresh data when the page appears
                await _viewModel.LoadDataAsync();


               
                // Optionally update UI elements or display messages
                if (_viewModel.CardItems == null || !_viewModel.CardItems.Any())
                {
                    await DisplayAlert("Info", "Your list is empty. Add items to see them here.", "OK");
                }
               

            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to load data: {ex.Message}", "OK");
            }
        }
    }


    //protected async override void OnAppearing()
    //{
    //    base.OnAppearing();


    //    if (GlobalSettings.SelectedGame != null)
    //    {
    //        InitializeComponent();

    //    }
    //    else
    //    {



    //        await Application.Current.MainPage.DisplayAlert("Error", "No game selected. Please select a game before accessing the search page.", "OK");
    //        await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack

    //        await Shell.Current.GoToAsync("//home");

    //    }
    //}


}


