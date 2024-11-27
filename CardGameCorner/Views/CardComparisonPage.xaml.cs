//using CardGameCorner.ViewModels;
//using CommunityToolkit.Mvvm.Input;

//namespace CardGameCorner.Views;

//public partial class CardComparisonPage : ContentPage
//{
//	public CardComparisonPage(CardComparisonViewModel viewmodel)
//	{
//		InitializeComponent();

//        BindingContext = viewmodel;
//        scannedimage.Source = viewmodel.ScannedImage;

//    }

//    private async void Retry(object sender, EventArgs e)
//    {
//        // Navigate to ScanPage
//        await Navigation.PushAsync(new ScanPage());
//    }


//}

using CardGameCorner.ViewModels;

namespace CardGameCorner.Views;

public partial class CardComparisonPage : ContentPage
{
    public CardComparisonPage(CardComparisonViewModel viewModel)
    {
        InitializeComponent();

        BindingContext = viewModel;

        // Load Image
        if (viewModel.ScannedImage != null)
        {
            //scannedimage.Source = viewModel.ScannedImage;
            scannedimage.Source = viewModel.ScannedImage;
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is CardComparisonViewModel viewModel)
        {
            // Re-assign the ScannedImage if it was previously cleared
            //scannedimage.Source = viewModel.ScannedImage;
            scannedimage.Source = viewModel.ScannedImage ?? "pokemon_card.png"; // Replace 'default_image.png' with your static image file.

        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        // Clean up scannedimage
        //if (scannedimage?.Source != null)
        //{
        //    scannedimage.Source = null;
        //    scannedimage = null;        
        //}
    }

    private async void Retry(object sender, EventArgs e)
    {
        if (Navigation.NavigationStack.LastOrDefault()?.GetType() != typeof(ScanPage))
        {
            await Navigation.PushAsync(new ScanPage());
        }
    }
}
