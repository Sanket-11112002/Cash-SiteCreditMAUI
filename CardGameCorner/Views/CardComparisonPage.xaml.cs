using CardGameCorner.ViewModels;
using CommunityToolkit.Mvvm.Input;

namespace CardGameCorner.Views;

public partial class CardComparisonPage : ContentPage
{
	public CardComparisonPage(CardComparisonViewModel viewmodel)
	{
		InitializeComponent();

        BindingContext = viewmodel;
        scannedimage.Source = viewmodel.ScannedImage;

    }


    private async void Retry(object sender, EventArgs e)
    {
        // Navigate to ScanPage
        await Navigation.PushAsync(new ScanPage());
    }


}