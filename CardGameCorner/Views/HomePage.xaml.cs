using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _viewModel;

    public HomePage(HomeViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
       
        await _viewModel.LoadGamesCommand.ExecuteAsync(null);

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
