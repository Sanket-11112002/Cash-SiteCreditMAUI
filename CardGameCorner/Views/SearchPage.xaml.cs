using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Camera;
using System.Diagnostics;
namespace CardGameCorner.Views;
public partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _viewModel;
    public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
    public SearchPage(SearchViewModel viewModel)
    {
        //if (GlobalSettings.SelectedGame != null)
        //{
        //    InitializeComponent();
        //    _viewModel = viewModel;
        //    BindingContext = _viewModel;
        //}
        //else
        //{
        //    Application.Current.MainPage.DisplayAlert("Error", "No game selected. Please select a game before accessing the search page.", "OK");
        //}

        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    //private void OnSearchButtonPressed(object sender, EventArgs e)
    //{
    //    // Get the ViewModel
    //    var viewModel = (SearchViewModel)BindingContext;

    //    // Trigger the search
    //    viewModel.SearchCommand.Execute(null);
    //}

    private async void OnScanButtonClicked(object sender, EventArgs e)
    {
        // Trigger camera scanning logic
        // await Navigation.PushAsync(new ScanPage());
        await Shell.Current.GoToAsync("//ScanPage");
    }
    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        // await Navigation.PushAsync(new SearchQueryPage());
        await Shell.Current.GoToAsync(nameof(SearchQueryPage));
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();
        if (GlobalSettings.SelectedGame != null)
        {
            try
            {
                await _viewModel.LoadBannerImage(GlobalSettings.SelectedGame);
                // Force refresh binding context if needed
                BindingContext = null;
                BindingContext = _viewModel;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading banner: {ex.Message}");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No game selected. Please select a game before accessing the search page.", "OK");
            await Shell.Current.Navigation.PopToRootAsync();
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        Console.WriteLine("SearchPage is disappearing.");
    }

    //protected override void OnDisappearing()
    //{
    //    base.OnDisappearing();

    //    // Check if we're navigating away from the Search tab
    //    if (Shell.Current.CurrentState.Location.OriginalString.Contains("///search") == false)
    //    {
    //        // Clear the navigation stack
    //        MainThread.BeginInvokeOnMainThread(async () =>
    //        {
    //            if (Navigation.NavigationStack.Count > 1)
    //            {
    //                await Navigation.PopToRootAsync(false);
    //            }
    //        });
    //    }
    //}

    protected override bool OnBackButtonPressed()
    {

        Task<bool> answer = DisplayAlert(AppResources.Exit,AppResources.ExitApp, AppResources.YesMsg, "No");
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