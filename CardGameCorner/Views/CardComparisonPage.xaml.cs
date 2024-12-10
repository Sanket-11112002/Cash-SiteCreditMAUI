
using CardGameCorner.Services;
using CardGameCorner.ViewModels;


namespace CardGameCorner.Views;

public partial class CardComparisonPage : ContentPage
{
    //public CardComparisonPage(List<CardComparisonViewModel> ComparisonData)
    //{
    //	InitializeComponent();

    //       BindingContext = ComparisonData;
    //       for(int i=0;i< ComparisonData.Count; i++)
    //       {
    //           scannedimage.Source = ComparisonData[i].ScannedImage;
    //           searchimage.Source = ComparisonData[i].SearchResultImage;
    //       }   
    //      // scannedimage.Source = ComparisonData.ScannedImage;
    //       searchimage.Source = "https://www.cardgamecorner.com/prodotti/1/1115/IMA006.jpg";


    //   }

    public CardComparisonPage(CardComparisonViewModel ComparisonData)
    {
        InitializeComponent();

        BindingContext = ComparisonData;
       
       scannedimage.Source = ComparisonData.ScannedImage;
       searchimage.Source = ComparisonData.SearchResultImage;
       searchimage1.Source = ComparisonData.SearchResultImage;
       searchimage2.Source = ComparisonData.SearchResultImage;
       
     


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
        // Navigate to ScanPage
        await Navigation.PushAsync(new ScanPage());
    }

    //protected override async void OnAppearing()
    //{
    //    base.OnAppearing();

    //    if (BindingContext is CardComparisonViewModel viewModel)
    //    {
    //        // Re-assign the ScannedImage if it was previously cleared
    //        //scannedimage.Source = viewModel.ScannedImage;
    //        if (viewModel.ScannedImage == null)
    //        {
    //            // Clear the navigation stack and navigate to the ScanPage
    //            await Shell.Current.GoToAsync($"//{nameof(ScanPage)}", true);
    //        } // Replace 'default_image.png' with your static image file.

    //    }
      

         

    //}
//   protected override async  void OnDisappearing()
//{
//    base.OnDisappearing();

//        //if (BindingContext is CardComparisonViewModel viewModel)
//        //{
//        //    // Clear the ScannedImage when the page disappears (if that's the intention)
//        //    viewModel.ScannedImage = null;
//        //}
//        await Shell.Current.GoToAsync($"/{nameof(SearchPage)}", true);
//    }




}