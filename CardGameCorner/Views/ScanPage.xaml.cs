
//using System.Text.Json;
//using CardGameCorner.Models;
//using CardGameCorner.Services;
//using CardGameCorner.ViewModels;

//namespace CardGameCorner.Views
//{
//    public partial class ScanPage : ContentPage
//    {

//        private readonly HttpClient _httpClient;
//        private readonly IScanCardService _scanCardService;
//        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
//        public ScanPage()
//        {
//            InitializeComponent();
//            _httpClient = new HttpClient();


//            // Inject the ViewModel with the ApiService
//            BindingContext = new ScanCardViewModel(new ScanCardService());
//        }



//        private async void OnCaptureButtonClicked(object sender, EventArgs e)
//        {
//            try
//            {
//                // Capture an image
//                var photo = await MediaPicker.CapturePhotoAsync();
//                if (photo != null)
//                {
//                    // Open the photo as a stream
//                    using (var originalStream = await photo.OpenReadAsync())
//                    {

//                        var viewModel = BindingContext as ScanCardViewModel;

//                        var compressedImageStream = await viewModel.CompressImageAsync(originalStream, 100 * 1024);

//                        var displayStream = new MemoryStream();
//                        compressedImageStream.Position = 0; // Reset position before copying
//                        await compressedImageStream.CopyToAsync(displayStream);
//                        displayStream.Position = 0; // Reset position for reading in the UI

//                        // Display the compressed image in the Image control
//                        capturedImage.Source = ImageSource.FromStream(() => displayStream);
//                        capturedImage.IsVisible = true;


//                        using (var httpClient = new HttpClient())
//                        using (var multipartContent = new MultipartFormDataContent())
//                        {
//                            // Create a copy of the compressed stream for upload
//                            var uploadStream = new MemoryStream();
//                            compressedImageStream.Position = 0; // Reset position before copying
//                            await compressedImageStream.CopyToAsync(uploadStream);
//                            uploadStream.Position = 0; // Reset position for upload

//                            var apiResponse = await viewModel.UploadImageAsync(uploadStream);
//                            if (apiResponse != null)
//                            {
//                                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//                                Console.WriteLine($"Upload successful: {apiResponse}");

//                                var cardRequest = new CardSearchRequest
//                                {
//                                    Title = apiResponse.Result.Title,
//                                    Set = apiResponse.Result.Set,
//                                    Game = GlobalSettings.SelectedGame,
//                                    Lang = apiResponse.Result.Lang,
//                                    Foil = apiResponse.Result.Foil,
//                                    FirstEdition = 0

//                                    //Title = "Angel of Mercy",//apiResponse.Result.Title,
//                                    //Set = "IMA",//apiResponse.Result.Set,
//                                    //Game = "magic",//"pokemon",
//                                    //Lang = "en",//apiResponse.Result.Lang,
//                                    //Foil = 0,//apiResponse.Result.Foil,
//                                    //FirstEdition = 0// 0

//                                };

//                                var data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

//                                if (data != null)
//                                {
//                                    await Navigation.PushAsync(new CardComparisonPage(data));
//                                }
//                                else
//                                {
//                                    await DisplayAlert("Error", "Card Not Found", "OK");
//                                }


//                            }
//                            else
//                            {
//                                await DisplayAlert("Error", "Card Not Found", "OK");
//                                viewModel.IsLoading = false;
//                            }
//                        }

//                        // Prepare the card search request
//                    }
//                }

//                else
//                {
//                    Console.WriteLine("No image captured.");

//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"An error occurred: {ex.Message}");
//            }
//        }



//        private async void OnUploadButtonClicked(object sender, EventArgs e)
//        {
//            try
//            {
//                // Open the file picker to select an image
//                var result = await FilePicker.PickAsync(new PickOptions
//                {
//                    PickerTitle = "Select an image",
//                    FileTypes = FilePickerFileType.Images // Restrict to image files
//                });

//                if (result != null)
//                {
//                    // Open the file stream
//                    using var originalStream = await result.OpenReadAsync();



//                    // Compress the image
//                    var viewModel = BindingContext as ScanCardViewModel;

//                    var compressedImageStream = await viewModel.CompressImageAsync(originalStream, 100 * 1024);

//                    // Clone the compressed stream for display
//                    var displayStream = new MemoryStream();
//                    compressedImageStream.Position = 0; // Reset position
//                    await compressedImageStream.CopyToAsync(displayStream);
//                    displayStream.Position = 0; // Reset for display

//                    // Display the image in the Image control
//                    capturedImage.Source = ImageSource.FromStream(() => displayStream);
//                    capturedImage.IsVisible = true;

//                    // Clone the compressed stream for upload
//                    var uploadStream = new MemoryStream();
//                    compressedImageStream.Position = 0; // Reset position
//                    await compressedImageStream.CopyToAsync(uploadStream);
//                    uploadStream.Position = 0; // Reset for upload

//                    // Upload the cloned stream to the API
//                    var apiResponse = await viewModel.UploadImageAsync(uploadStream);

//                    if (apiResponse != null)
//                    {
//                        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//                        Console.WriteLine($"Upload successful: {apiResponse}");

//                        var cardRequest = new CardSearchRequest
//                        {
//                            //Title = apiResponse.Result.Title,
//                            //Set = apiResponse.Result.Set,
//                            //Game = GlobalSettings.SelectedGame,
//                            //Lang = apiResponse.Result.Lang,
//                            //Foil = apiResponse.Result.Foil,
//                            //FirstEdition = 0

//                            Title = "Angel of Mercy",//apiResponse.Result.Title,
//                            Set = "IMA",//apiResponse.Result.Set,
//                            Game = "magic",//"pokemon",
//                            Lang = "en",//apiResponse.Result.Lang,
//                            Foil = 0,//apiResponse.Result.Foil,
//                            FirstEdition = 0// 0


//                        };

//                        var data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

//                        if (data != null)
//                        {
//                            await Navigation.PushAsync(new CardComparisonPage(data));
//                        }
//                        else
//                        {
//                            await DisplayAlert("Error", "Card Not Found", "OK");
//                        }


//                    }
//                    else
//                    {
//                        await DisplayAlert("Error", "Card Not Found", "OK");
//                        viewModel.IsLoading = false;
//                    }
//                }
//                else
//                {
//                    await DisplayAlert("Cancelled", "No image selected.", "OK");
//                }
//            }
//            catch (Exception ex)
//            {
//                await DisplayAlert("Error", $"Failed to select or upload image: {ex.Message}", "OK");
//                Console.WriteLine($"Error: {ex.Message}");
//            }
//        }



//        //private async Task UploadImageToApi(Stream imageStream)
//        //{
//        //    try
//        //    {
//        //        imageStream.Position = 0; // Ensure stream starts from the beginning
//        //        long imageSizeBytes = imageStream.Length; // Get size in bytes
//        //        double imageSizeMB = imageSizeBytes / (1024.0 * 1024.0); // Convert bytes to MB
//        //        Console.WriteLine($"Image size: {imageSizeMB:F2} MB");

//        //        using var httpClient = new HttpClient();
//        //        using var multipartContent = new MultipartFormDataContent();
//        //        var fileContent = new StreamContent(imageStream);
//        //        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

//        //        // Add file to the multipart content
//        //        multipartContent.Add(fileContent, "image", "uploaded_image.jpg");

//        //        // API Endpoint
//        //        string game = "pokemon";
//        //        string apiKey = "0d66cf7894c3ed46592332829e6d467b";
//        //        string url = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

//        //        // Send the POST request
//        //        var response = await httpClient.PostAsync(url, multipartContent);
//        //        var responseContent = await response.Content.ReadAsStringAsync();

//        //        var displayStream = new MemoryStream();

//        //        await imageStream.CopyToAsync(displayStream);
//        //        displayStream.Position = 0; // Reset position for reading in the UI

//        //        if (response.IsSuccessStatusCode)
//        //        {
//        //           // await DisplayAlert("Success", "Image uploaded successfully!", "OK");
//        //            Console.WriteLine($"Upload successful: {responseContent}");


//        //            // Deserialize the response into ScannedCardDetails
//        //            var options = new JsonSerializerOptions
//        //            {
//        //                PropertyNameCaseInsensitive = true
//        //            };



//        //            //  ScannedCardDetails scannedCardDetails = JsonSerializer.Deserialize<ScannedCardDetails>(responseContent)!;
//        //            //var scannedCardDetails = JsonSerializer.Deserialize<ScannedCardDetails>(responseContent, options);

//        //            var apiResponse = JsonSerializer.Deserialize<ApiResponse_Card>(responseContent, options);


//        //         //   var comparisonData = new CardComparisonViewModel();


//        //           // comparisonData.Initialize(apiResponse, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));
//        //          //  comparisonData.Initialize(apiResponse, ImageSource.FromStream(() => displayStream));


//        //            // Navigate to the CardComparisonPage

//        //            // Navigate to the page with the initialized viewModel
//        //           // await Navigation.PushAsync(new CardComparisonPage(comparisonData));
//        //        }
//        //        else
//        //        {
//        //            await DisplayAlert("Error", $"Upload failed with status code: {response.StatusCode}", "OK");
//        //            Console.WriteLine($"Upload failed: {response.StatusCode}");
//        //        }
//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        await DisplayAlert("Error", $"Failed to upload image: {ex.Message}", "OK");
//        //        Console.WriteLine($"Error during upload: {ex.Message}");
//        //    }
//        //}

//    }
//}


using System.Text.Json;
using CardGameCorner.Models;
using CardGameCorner.Resources.Language;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using SkiaSharp;

namespace CardGameCorner.Views
{
    public partial class ScanPage : ContentPage
    {
        private readonly IScanCardService _scanCardService;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;

        public ScanPage()
        {
            InitializeComponent();
            _scanCardService = new ScanCardService();
            BindingContext = new ScanCardViewModel(_scanCardService);
        }

        private async void OnCaptureButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var viewModel = BindingContext as ScanCardViewModel;
                viewModel.IsLoading = true;

                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    using (var originalStream = await photo.OpenReadAsync())
                    {
                        // Correct image orientation
                      //  var correctedStream = await CorrectImageOrientationAsync(originalStream);

                        var compressedImageStream = await viewModel.CompressImageAsync(originalStream, 100 * 1024);

                        if (compressedImageStream != null)
                        {
                            var displayStream = new MemoryStream();
                            compressedImageStream.Position = 0;
                            await compressedImageStream.CopyToAsync(displayStream);
                            displayStream.Position = 0;

                            // Display the compressed image in the Image control
                            capturedImage.Source = ImageSource.FromStream(() => displayStream);
                            capturedImage.IsVisible = true;

                            var uploadStream = new MemoryStream();
                            compressedImageStream.Position = 0;
                            await compressedImageStream.CopyToAsync(uploadStream);
                            uploadStream.Position = 0;

                            var apiResponse = await viewModel.UploadImageAsync(uploadStream);
                            if (apiResponse != null)
                            {
                                var cardRequest = new CardSearchRequest
                                {
                                    //Title = apiResponse.Result.Title,
                                    //Set = apiResponse.Result.Set,
                                    //Game = GlobalSettings.SelectedGame,
                                    //Lang = apiResponse.Result.Lang,
                                    //Foil = apiResponse.Result.Foil,
                                    //FirstEdition = 0

                                    Title = "Angel of Mercy",
                                    Set = "magic",
                                    Game = "IMA",
                                    Lang = "en",
                                    Foil = 0,
                                    FirstEdition = 0

                                };

                                var data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

                                if (data != null)
                                {
                                    await Navigation.PushAsync(new CardComparisonPage(data));
                                }
                                else
                                {
                                    await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                                }
                            }
                            else
                            {
                                await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                        }
                    }
                }
                else
                {
                    Console.WriteLine("No image captured.");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
            finally
            {
                var viewModel = BindingContext as ScanCardViewModel;
                viewModel.IsLoading = false;
            }
        }

        private async Task<Stream> CorrectImageOrientationAsync(Stream inputStream)
        {
            try
            {
                // Decode the input stream into a SkiaSharp bitmap
                using var skBitmap = SKBitmap.Decode(inputStream);

                // Create a new surface with rotated dimensions (swap width and height)
                using var surface = SKSurface.Create(new SKImageInfo(skBitmap.Height, skBitmap.Width));
                var canvas = surface.Canvas;

                // Rotate the image by 90 degrees
                canvas.RotateDegrees(90, skBitmap.Height / 2f, skBitmap.Height / 2f);

                // Draw the original image onto the canvas at the rotated position
                canvas.DrawBitmap(skBitmap, 0, 0);

                // Encode the rotated bitmap back to a stream
                using var rotatedImage = surface.Snapshot();
                using var rotatedData = rotatedImage.Encode(SKEncodedImageFormat.Jpeg, 100);

                var resultStream = new MemoryStream();
                rotatedData.SaveTo(resultStream);
                resultStream.Position = 0;

                return resultStream;
            }
            catch
            {
                // If an error occurs, return the original stream
                inputStream.Position = 0;
                return inputStream;
            }
        }

        private async void OnUploadButtonClicked(object sender, EventArgs e)
        {
            try
            {
                var viewModel = BindingContext as ScanCardViewModel;
                viewModel.IsLoading = true;

                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images
                });

                if (result != null)
                {
                    using var originalStream = await result.OpenReadAsync();

                    var compressedImageStream = await viewModel.CompressImageAsync(originalStream, 100 * 1024);

                    if (compressedImageStream != null)
                    {
                        var displayStream = new MemoryStream();
                        compressedImageStream.Position = 0;
                        await compressedImageStream.CopyToAsync(displayStream);
                        displayStream.Position = 0;

                        capturedImage.Source = ImageSource.FromStream(() => displayStream);
                        capturedImage.IsVisible = true;

                        var uploadStream = new MemoryStream();
                        compressedImageStream.Position = 0;
                        await compressedImageStream.CopyToAsync(uploadStream);
                        uploadStream.Position = 0;

                        var apiResponse = await viewModel.UploadImageAsync(uploadStream);

                        if (apiResponse != null)
                        {


                            var cardRequest = new CardSearchRequest
                            {
                                Title = apiResponse.Result.Title,
                                Set = apiResponse.Result.Set,
                                Game = GlobalSettings.SelectedGame,
                                Lang = apiResponse.Result.Lang,
                                Foil = apiResponse.Result.Foil,
                                FirstEdition = 0
                            };

                            var data = await viewModel.SearchCardAsync(cardRequest, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

                            if (data != null)
                            {
                                await Navigation.PushAsync(new CardComparisonPage(data));
                            }
                            else
                            {
                                await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                            }
                        }
                        else
                        {
                            await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                        }
                    }
                    else
                    {
                        await DisplayAlert(AppResources.ErrorTitle, AppResources.CardNotFound, "OK");
                    }
                }
                else
                {
                    await DisplayAlert("Cancelled", "No image selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to select or upload image: {ex.Message}", "OK");
            }
            finally
            {
                var viewModel = BindingContext as ScanCardViewModel;
                viewModel.IsLoading = false;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Reset image and visibility when returning to the page
            capturedImage.Source = null;
            capturedImage.IsVisible = false;
        }
    }
}