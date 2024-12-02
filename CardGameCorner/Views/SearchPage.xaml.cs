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


    

}