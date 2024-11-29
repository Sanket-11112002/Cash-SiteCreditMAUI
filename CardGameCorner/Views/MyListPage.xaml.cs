using CardGameCorner.Services;
using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

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

}