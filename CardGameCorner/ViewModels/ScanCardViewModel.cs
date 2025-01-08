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
    public partial class ScanCardViewModel : BaseViewModel, INotifyPropertyChanged
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
                // Create a copy of the input stream to prevent disposal issues
                var memoryStream = new MemoryStream();
                await inputStream.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                // Read EXIF orientation
                int orientation = 1; // Default orientation
                try
                {
                    // Create a separate stream for EXIF reading
                    var exifStream = new MemoryStream(memoryStream.ToArray());
                    using (var codec = SKCodec.Create(exifStream))
                    {
                        if (codec != null)
                        {
                            orientation = GetExifOrientation(codec);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to read EXIF: {ex.Message}");
                }

                // Decode the image
                using var originalBitmap = SKBitmap.Decode(memoryStream);
                if (originalBitmap == null)
                    throw new Exception("Failed to decode the input image.");

                // Apply rotation based on EXIF orientation
                using var rotatedBitmap = ApplyExifOrientation(originalBitmap, orientation);

                // Determine max width based on device capabilities
                var width = Math.Min(800, rotatedBitmap.Width);
                var height = (int)((float)rotatedBitmap.Height * width / rotatedBitmap.Width);

                // Resize the image
                using var resizedImage = rotatedBitmap.Resize(new SKImageInfo(width, height), SKFilterQuality.High);
                if (resizedImage == null)
                    throw new Exception("Failed to resize the image.");

                // Compress with quality adjustment
                var quality = 85;
                MemoryStream finalCompressedStream = null;

                do
                {
                    using var tempStream = new MemoryStream();
                    using (var skImage = SKImage.FromBitmap(resizedImage))
                    using (var data = skImage.Encode(SKEncodedImageFormat.Jpeg, quality))
                    {
                        data.SaveTo(tempStream);
                    }

                    if (tempStream.Length <= maxSize)
                    {
                        // We found a good compression level, save this stream
                        finalCompressedStream = new MemoryStream();
                        tempStream.Position = 0;
                        await tempStream.CopyToAsync(finalCompressedStream);
                        break;
                    }

                    quality -= 5;
                } while (quality > 10);

                // Clean up the input memory stream
                memoryStream.Dispose();

                if (finalCompressedStream != null)
                {
                    finalCompressedStream.Position = 0;
                    return finalCompressedStream;
                }

                throw new Exception("Failed to compress image to desired size");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image compression failed: {ex.Message}");
                return null;
            }
        }

        private int GetExifOrientation(SKCodec codec)
        {
            try
            {
                var orientation = codec.EncodedOrigin;
                switch (orientation)
                {
                    case SKEncodedOrigin.TopLeft: return 1;
                    case SKEncodedOrigin.TopRight: return 2;
                    case SKEncodedOrigin.BottomRight: return 3;
                    case SKEncodedOrigin.BottomLeft: return 4;
                    case SKEncodedOrigin.LeftTop: return 5;
                    case SKEncodedOrigin.RightTop: return 6;
                    case SKEncodedOrigin.RightBottom: return 7;
                    case SKEncodedOrigin.LeftBottom: return 8;
                    default: return 1;
                }
            }
            catch
            {
                return 1;
            }
        }

        private SKBitmap ApplyExifOrientation(SKBitmap original, int orientation)
        {
            if (orientation == 1)
                return original;

            // Create new bitmap with correct dimensions
            SKImageInfo info = original.Info;

            // Adjust dimensions for rotation
            if (orientation > 4)
            {
                info = new SKImageInfo(info.Height, info.Width, info.ColorType, info.AlphaType);
            }

            var rotated = new SKBitmap(info);

            using (var surface = new SKCanvas(rotated))
            {
                surface.Clear();

                switch (orientation)
                {
                    case 2: // flip horizontal
                        surface.Scale(-1, 1, info.Width / 2f, 0);
                        break;
                    case 3: // 180° rotation
                        surface.RotateDegrees(180, info.Width / 2f, info.Height / 2f);
                        break;
                    case 4: // flip vertical
                        surface.Scale(1, -1, 0, info.Height / 2f);
                        break;
                    case 5: // flip vertical + 90° rotation
                        surface.RotateDegrees(90);
                        surface.Scale(1, -1, 0, 0);
                        break;
                    case 6: // 90° rotation
                        surface.RotateDegrees(90);
                        surface.Translate(0, -original.Height);
                        break;
                    case 7: // flip horizontal + 90° rotation
                        surface.RotateDegrees(90);
                        surface.Scale(-1, 1, info.Width / 2f, 0);
                        break;
                    case 8: // 270° rotation
                        surface.RotateDegrees(270);
                        surface.Translate(-original.Width, 0);
                        break;
                }

                surface.DrawBitmap(original, 0, 0);
            }

            return rotated;
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
