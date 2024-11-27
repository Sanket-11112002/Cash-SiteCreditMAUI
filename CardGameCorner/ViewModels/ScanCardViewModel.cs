using CardGameCorner.Services;
using CardGameCorner.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls;
using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;
using SkiaSharp;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CardGameCorner.Views;
using System.Net.Http;
using System.Text;


namespace CardGameCorner.ViewModels
{
    public partial class ScanCardViewModel : ObservableObject
    {
        private readonly IScanCardService _scanCardService;

        [ObservableProperty]
        private ImageSource capturedImage;

        public ScanCardViewModel(IScanCardService scanCardService)
        {
            _scanCardService = scanCardService;
        }

        //[RelayCommand]
        //public async Task CaptureImageCommand(FileResult fileResult)
        //{
        //    if (fileResult != null)
        //    {
        //        try
        //        {
        //            // Compress image and get the display stream
        //            var displayStream = await _scanCardService.CompressImageAsync(await fileResult.OpenReadAsync(), 100 * 1024);
        //            CapturedImage = ImageSource.FromStream(() => displayStream);

        //            // Upload image to API
        //            var apiResponse = await _scanCardService.UploadImageAsync(displayStream);
        //            if (apiResponse != null)
        //            {
        //                // Navigate to CardComparisonPage with response
        //                await Shell.Current.GoToAsync($"{nameof(CardComparisonPage)}", true, new Dictionary<string, object>
        //                {
        //                    { "apiResponse", apiResponse },
        //                    { "imageBytes", displayStream.ToArray() }
        //                });
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        //        }
        //    }
        //}

        // The method to compress the image

        public IAsyncRelayCommand CaptureImageCommand { get; }

        public ScanCardViewModel()
        {
           // CaptureImageCommand = new AsyncRelayCommand<FileResult>(CaptureImage);
        }

        // The asynchronous method called when the command is executed
        //public async Task CaptureImage(FileResult fileResult)
        //{
        //    if (fileResult != null)
        //    {
        //        try
        //        {
        //            // Compress image and get the display stream
        //            var displayStream = await _scanCardService.CompressImageAsync(await fileResult.OpenReadAsync(), 100 * 1024);
        //            CapturedImage = ImageSource.FromStream(() => displayStream);

        //            // Upload image to API
        //            var apiResponse = await _scanCardService.UploadImageAsync(displayStream);
        //            if (apiResponse != null)
        //            {
        //                // Navigate to CardComparisonPage with response and image bytes
        //                var navigationParameters = new Dictionary<string, object>
        //        {
        //            { "apiResponse", apiResponse },
        //            { "imageBytes", displayStream.ToArray() }
        //        };

        //                // Passing parameters to the CardComparisonPage
        //                await Shell.Current.GoToAsync($"{nameof(CardComparisonPage)}", true, navigationParameters);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        //        }
        //    }
        //}

        public  async Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        {
            try
            {
                using var skiaImage = SKBitmap.Decode(inputStream);
                if (skiaImage == null)
                    throw new Exception("Failed to decode the input image.");

                var width = 800; // Target width
                var height = (int)((float)skiaImage.Height * width / skiaImage.Width);
                var resizedImage = skiaImage.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);

                if (resizedImage == null)
                    throw new Exception("Failed to resize the image.");

                var quality = 85;
                MemoryStream compressedStream;
                do
                {
                    compressedStream = new MemoryStream();
                    using (var skImage = SKImage.FromBitmap(resizedImage))
                    {
                        var skData = skImage.Encode(SKEncodedImageFormat.Jpeg, quality);
                        skData.SaveTo(compressedStream);
                    }

                    if (compressedStream.Length <= maxSize)
                        break;

                    quality -= 5;
                    compressedStream.Dispose();
                } while (quality > 0);

                compressedStream.Position = 0;
                return compressedStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image compression failed: {ex.Message}");
                return null;
            }
        }



        public async Task<CardComparisonViewModel> SearchCardAsync(CardSearchRequest cardSearch,ImageSource imageSource)
        {
            CardSearchResponseViewModel cardSearchResponseViewModel = await _scanCardService.SearchCardAsync(cardSearch);

            if (cardSearchResponseViewModel != null && cardSearchResponseViewModel.Products.Count > 0)
            {
                // Here, you can access and display data from the response
                var product = cardSearchResponseViewModel.Products[0];
                Console.WriteLine($"Product Model: {product.ModelEn}, Price: {product.MinPrice}");

                // Initialize the CardComparisonPage ViewModel
                CardComparisonViewModel comparisonData = new CardComparisonViewModel();

                // Initialize the comparison data (you can pass the full response or use specific data)
                 comparisonData.Initialize(cardSearchResponseViewModel, imageSource);

                return comparisonData;


                //await Shell.Current.GoToAsync(nameof(CardComparisonPage), new Dictionary<string, object>
                //{
                //    { "ComparisonData", comparisonData }
                //});

        //        var navigationParameters = new Dictionary<string, object>
        //{
        //    { "ComparisonData", comparisonData }
        //};

        //        await Shell.Current.GoToAsync(nameof(CardComparisonPage), navigationParameters);






            }
            else
            {
                Console.WriteLine("No products found in card search.");
                return null;
            }
        }



        public async Task<ApiResponse_Card> UploadImageAsync(Stream imageStream)
        {
            var ApiResponse_Card=await _scanCardService.UploadImageAsync(imageStream);

            return ApiResponse_Card;
        }
    }


    
}
