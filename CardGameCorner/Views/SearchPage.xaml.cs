using CommunityToolkit.Maui.Camera;
namespace CardGameCorner.Views;


public partial class SearchPage : ContentPage
{


    public SearchPage()
    {
        InitializeComponent();
    }
    private async void OnScanButtonClicked(object sender, EventArgs e)
    {
        // Trigger camera scanning logic
        await Navigation.PushAsync(new ScanPage());    
    }
    private async void OnSearchButtonClicked(object sender, EventArgs e)
    {
        // Trigger camera scanning logic
        await Navigation.PushAsync(new SearchQueryPage());
    }




}