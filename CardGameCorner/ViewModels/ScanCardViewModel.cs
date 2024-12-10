//using CardGameCorner.Models;
//using CardGameCorner.Services;
//using CommunityToolkit.Maui.Views;
//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;


//namespace CardGameCorner.Views
//{
//    public partial class ScanCardViewModel : ObservableObject
//    {
//        private readonly IScanCardService _scanCardService;

//        [ObservableProperty]
//        private ImageSource capturedImageSource;

//        [ObservableProperty]
//        private bool isImageVisible;

//        [ObservableProperty]
//        private string apiResponse;

//        public ScanCardViewModel(IScanCardService scanCardService)
//        {
//            _scanCardService = scanCardService;
//        }

//        [RelayCommand]
//        public async Task HandleImageCaptured(MediaCapturedEventArgs e)
//        {
//            if (e?.Media == null) return;

//            try
//            {
//                // Convert stream to byte array
//                using var memoryStream = new MemoryStream();
//                await e.Media.CopyToAsync(memoryStream);
//                byte[] imageData = memoryStream.ToArray();

//                // Update UI with captured image
//                capturedImageSource = ImageSource.FromStream(() => new MemoryStream(imageData));
//                isImageVisible = true;

//                // Upload to API
//                apiResponse = await _scanCardService.UploadImageAsync(imageData);

//                await Application.Current.MainPage.DisplayAlert("Success", "Image processed successfully!", "OK");
//            }
//            catch (Exception ex)
//            {
//                await Application.Current.MainPage.DisplayAlert("Error", $"Failed to process image: {ex.Message}", "OK");
//            }
//        }
//    }
//}


using CardGameCorner.Services;
using CardGameCorner.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SkiaSharp;
using CardGameCorner.Views;
using CardGameCorner.Resources.Language;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace CardGameCorner.ViewModels
{
    public partial class ScanCardViewModel :BaseViewModel, INotifyPropertyChanged
    {
        private readonly IScanCardService _scanCardService;

        [ObservableProperty]
        private ImageSource capturedImage;

        [ObservableProperty]
        private bool isLoading;

        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        [ObservableProperty]
        private string captureImage;

        [ObservableProperty]
        private string uploadImage;

        public ScanCardViewModel(IScanCardService scanCardService)
        {
            UpdateLocalizedStrings();
            
            GlobalSettings.PropertyChanged += OnGlobalSettingsPropertyChanged;
            _scanCardService = scanCardService;
        }
        private void OnGlobalSettingsPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GlobalSettings.SelectedLanguage))
            {
                UpdateLocalizedStrings();
            }
        }

        private void UpdateLocalizedStrings()
        {
            MainThread.BeginInvokeOnMainThread(() =>
            {
                CaptureImage = AppResources.Capture_Image; 
                UploadImage = AppResources.Upload_Image; 
                
                OnPropertyChanged(nameof(CaptureImage));
                OnPropertyChanged(nameof(UploadImage));
                
            });
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        [RelayCommand]
        public async Task CaptureImageAsync(FileResult fileResult)
        {
            if (fileResult == null) return;

            try
            {
                IsLoading = true;

                var displayStream = await CompressImageAsync(await fileResult.OpenReadAsync(), 100 * 1024);

                if (displayStream == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Failed to process image", "OK");
                    return;
                }

                CapturedImage = ImageSource.FromStream(() => displayStream);

                var cardSearchRequest = new CardSearchRequest
                {
                    
                };

                var comparisonData = await SearchCardAsync(cardSearchRequest, CapturedImage);

                if (comparisonData != null)
                {
                    await Shell.Current.GoToAsync(nameof(CardComparisonPage), new Dictionary<string, object>
                {
                    { "ComparisonData", comparisonData }
                });
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "No card information found", "OK");
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        {
            try
            {
                IsLoading = true;
                using var skiaImage = SKBitmap.Decode(inputStream);
                if (skiaImage == null)
                    throw new Exception("Failed to decode the input image.");

                var width = 800;
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

        public async Task<CardComparisonViewModel> SearchCardAsync(CardSearchRequest cardSearch, ImageSource imageSource)
        {
            try
            {
                var cardSearchResponseList = await _scanCardService.SearchCardAsync(cardSearch);

                if (cardSearchResponseList != null && cardSearchResponseList.Any())
                {
                    foreach (var cardSearchResponseViewModel in cardSearchResponseList)
                    {
                        if (cardSearchResponseViewModel.Products != null && cardSearchResponseViewModel.Products.Any())
                        {
                            foreach (var product in cardSearchResponseViewModel.Products)
                            {
                                Console.WriteLine($"Product Model: {product.ModelEn}, Price: {product.MinPrice}");
                            }

                            var comparisonData = new CardComparisonViewModel();
                            comparisonData.Initialize(cardSearchResponseViewModel, imageSource);

                            IsLoading = false;
                            return comparisonData; 
                        }
                    }

                    Console.WriteLine("No products found in the card search responses.");
                }
                else
                {
                    Console.WriteLine("No card search responses found.");
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Card search failed: {ex.Message}");
                return null;
            }
            finally
            {
                IsLoading = false;
            }
        }

        public async Task<ApiResponse_Card> UploadImageAsync(Stream imageStream)
        {
            var apiResponseCard = await _scanCardService.UploadImageAsync(imageStream);
            return apiResponseCard;
        }
    }
}
