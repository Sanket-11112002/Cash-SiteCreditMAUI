using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Camera;
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

        //InitializeComponent();
        _viewModel = viewModel;
        //BindingContext = _viewModel;
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        // Get the ViewModel
        var viewModel = (SearchViewModel)BindingContext;

        // Trigger the search
        viewModel.SearchCommand.Execute(null);
    }

    private async void OnScanButtonClicked(object sender, EventArgs e)
    {
        // Trigger camera scanning logic
        await Navigation.PushAsync(new ScanPage());
    }
    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SearchQueryPage());
    }
    protected async override void OnAppearing()
    {
        base.OnAppearing();


        if (GlobalSettings.SelectedGame != null)
        {
            InitializeComponent();

            BindingContext = _viewModel;
        }
        else
        {
            


           await Application.Current.MainPage.DisplayAlert("Error", "No game selected. Please select a game before accessing the search page.", "OK");
            await Shell.Current.Navigation.PopToRootAsync(); // Clears the stack
          
            await Shell.Current.GoToAsync("//home");

        }

        Console.WriteLine("SearchPage is appearing.");
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        Console.WriteLine("SearchPage is disappearing.");
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


}