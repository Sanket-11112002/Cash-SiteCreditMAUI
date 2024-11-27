using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Camera;
namespace CardGameCorner.Views;
public partial class SearchPage : ContentPage
{
    private readonly SearchViewModel _viewModel;
    public SearchPage(SearchViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
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
    protected override void OnAppearing()
    {
        base.OnAppearing();
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