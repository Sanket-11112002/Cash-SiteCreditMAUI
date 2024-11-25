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
        //base.OnAppearing();
        await _viewModel.LoadGamesCommand.ExecuteAsync(null);
    }

    private async void OnSettingsClicked(object sender, EventArgs e)
    {
        // Display dropdown-like popup for settings
        string result = await DisplayActionSheet("Settings", "Cancel", null,
            "Select Language", "Select Game");

        switch (result)
        {
            case "Select Language":
                await HandleLanguageSelection();
                break;

            case "Select Game":
                await HandleGameSelection();
                break;
        }
    }

    private async Task HandleLanguageSelection()
    {
        string language = await DisplayActionSheet("Choose a Language", "Cancel", null,
            "English", "Italian");

        if (!string.IsNullOrEmpty(language) && language != "Cancel")
        {
            Console.WriteLine($"Selected Language: {language}");
        }
    }

    private async Task HandleGameSelection()
    {
        string game = await DisplayActionSheet("Choose a Game", "Cancel", null,
            "Pokémon", "One Piece", "Magic", "Yu-Gi-Oh!");

        if (!string.IsNullOrEmpty(game) && game != "Cancel")
        {
            Console.WriteLine($"Selected Game: {game}");
        }
    }
}
