using System.IO;
using System.Net.Http.Headers;
using System.Text.Json;

using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Views;
//using Java.Util.Streams;
using Microsoft.Maui.Controls;
using SkiaSharp;
using Stream = System.IO.Stream;

namespace CardGameCorner.Views
{
    public partial class ScanPage : ContentPage
    {
        public ScanPage()
        {
            InitializeComponent();

            // Inject the ViewModel with the ApiService
            BindingContext = new ScanCardViewModel(new ScanCardService());
        }

        private async void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            var viewModel = BindingContext as ScanCardViewModel;
            if (viewModel != null)
            {
                await viewModel.CaptureImageCommand.ExecuteAsync(e);
            }
        }
        private async void OnCaptureButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Capture an image
                var photo = await MediaPicker.CapturePhotoAsync();
                if (photo != null)
                {
                    // Open the photo as a stream
                    using (var originalStream = await photo.OpenReadAsync())
                    {
                        // Resize and compress the image to be under 100 KB
                        var compressedImageStream = await CompressImageAsync(originalStream, 100 * 1024); // 100 KB

                        // Clone the compressed stream for display purposes
                        var displayStream = new MemoryStream();
                        compressedImageStream.Position = 0; // Reset position before copying
                        await compressedImageStream.CopyToAsync(displayStream);
                        displayStream.Position = 0; // Reset position for reading in the UI

                        // Display the compressed image in the Image control
                        capturedImage.Source = ImageSource.FromStream(() => displayStream);
                        capturedImage.IsVisible = true;

                        // Prepare the HTTP client and multipart content
                        using (var httpClient = new HttpClient())
                        using (var multipartContent = new MultipartFormDataContent())
                        {
                            // Create a copy of the compressed stream for upload
                            var uploadStream = new MemoryStream();
                            compressedImageStream.Position = 0; // Reset position before copying
                            await compressedImageStream.CopyToAsync(uploadStream);
                            uploadStream.Position = 0; // Reset position for upload

                            // Add the compressed image to multipart content
                            var fileContent = new StreamContent(uploadStream);
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");
                            multipartContent.Add(fileContent, "image", photo.FileName);

                            // API endpoint and parameters
                            string game = "pokemon";
                            string apiKey = "0d66cf7894c3ed46592332829e6d467b";
                            string url = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

                            // Send the request
                            var response = await httpClient.PostAsync(url, multipartContent);
                            var responseContent = await response.Content.ReadAsStringAsync();

                            // Handle the response
                            if (response.IsSuccessStatusCode)
                            {
                                Console.WriteLine($"Upload successful: {responseContent}");

                                // Deserialize the response into ScannedCardDetails
                                var options = new JsonSerializerOptions
                                {
                                    PropertyNameCaseInsensitive = true
                                };



                                //  ScannedCardDetails scannedCardDetails = JsonSerializer.Deserialize<ScannedCardDetails>(responseContent)!;
                                //var scannedCardDetails = JsonSerializer.Deserialize<ScannedCardDetails>(responseContent, options);

                                var apiResponse = JsonSerializer.Deserialize<ApiResponse_Card>(responseContent, options);


                                var comparisonData = new CardComparisonViewModel();
                                 comparisonData.Initialize(apiResponse, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));

                                // Navigate to the CardComparisonPage

                                // Navigate to the page with the initialized viewModel
                                await Navigation.PushAsync(new CardComparisonPage(comparisonData));

                            }
                            else
                            {
                                Console.WriteLine($"Upload failed: {response.StatusCode}");
                                await DisplayAlert("Error", $"Card {response.ReasonPhrase}", "OK");
                            }
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
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }


        // Method to compress the image stream and ensure it is under the maxSize
        private async Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        {
            try
            {
                // Decode the input stream into an SKBitmap
                using var skiaImage = SKBitmap.Decode(inputStream);
                if (skiaImage == null)
                    throw new Exception("Failed to decode the input image.");

                int width = 800; // Set target width (adjustable)
                int height = (int)((float)skiaImage.Height * width / skiaImage.Width);

                // Resize the image
                var resizedImage = skiaImage.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);
                if (resizedImage == null)
                    throw new Exception("Failed to resize the image.");

                // Initialize compression quality
                int quality = 85;
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
                        break; // Compression is sufficient

                    quality -= 5; // Reduce quality further
                    compressedStream.Dispose(); // Dispose of the stream to retry
                }
                while (quality > 0);

                // Finalize the stream
                compressedStream.Position = 0;
                return compressedStream;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image compression failed: {ex.Message}");
                return null;
            }
        }

        private async void OnUploadButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Open the file picker to select an image
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Select an image",
                    FileTypes = FilePickerFileType.Images // Restrict to image files
                });

                if (result != null)
                {
                    // Open the file stream
                    using var originalStream = await result.OpenReadAsync();

                    // Compress the image
                    var compressedImageStream = await CompressImageAsync(originalStream, 100 * 1024); // 100 KB

                    // Clone the compressed stream for display
                    var displayStream = new MemoryStream();
                    compressedImageStream.Position = 0; // Reset position
                    await compressedImageStream.CopyToAsync(displayStream);
                    displayStream.Position = 0; // Reset for display

                    // Display the image in the Image control
                    capturedImage.Source = ImageSource.FromStream(() => displayStream);
                    capturedImage.IsVisible = true;

                    // Clone the compressed stream for upload
                    var uploadStream = new MemoryStream();
                    compressedImageStream.Position = 0; // Reset position
                    await compressedImageStream.CopyToAsync(uploadStream);
                    uploadStream.Position = 0; // Reset for upload

                    // Upload the cloned stream to the API
                    await UploadImageToApi(uploadStream);
                }
                else
                {
                    await DisplayAlert("Cancelled", "No image selected.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to select or upload image: {ex.Message}", "OK");
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private async Task UploadImageToApi(Stream imageStream)
        {
            try
            {
                imageStream.Position = 0; // Ensure stream starts from the beginning
                long imageSizeBytes = imageStream.Length; // Get size in bytes
                double imageSizeMB = imageSizeBytes / (1024.0 * 1024.0); // Convert bytes to MB
                Console.WriteLine($"Image size: {imageSizeMB:F2} MB");

                using var httpClient = new HttpClient();
                using var multipartContent = new MultipartFormDataContent();
                var fileContent = new StreamContent(imageStream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                // Add file to the multipart content
                multipartContent.Add(fileContent, "image", "uploaded_image.jpg");

                // API Endpoint
                string game = "pokemon";
                string apiKey = "0d66cf7894c3ed46592332829e6d467b";
                string url = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

                // Send the POST request
                var response = await httpClient.PostAsync(url, multipartContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                var displayStream = new MemoryStream();

                await imageStream.CopyToAsync(displayStream);
                displayStream.Position = 0; // Reset position for reading in the UI

                if (response.IsSuccessStatusCode)
                {
                   // await DisplayAlert("Success", "Image uploaded successfully!", "OK");
                    Console.WriteLine($"Upload successful: {responseContent}");
                    Console.WriteLine($"Upload successful: {responseContent}");

                    // Deserialize the response into ScannedCardDetails
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };



                    //  ScannedCardDetails scannedCardDetails = JsonSerializer.Deserialize<ScannedCardDetails>(responseContent)!;
                    //var scannedCardDetails = JsonSerializer.Deserialize<ScannedCardDetails>(responseContent, options);

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse_Card>(responseContent, options);


                    var comparisonData = new CardComparisonViewModel();
                   // comparisonData.Initialize(apiResponse, ImageSource.FromStream(() => new MemoryStream(displayStream.ToArray())));
                    comparisonData.Initialize(apiResponse, ImageSource.FromStream(() => displayStream));


                    // Navigate to the CardComparisonPage

                    // Navigate to the page with the initialized viewModel
                    await Navigation.PushAsync(new CardComparisonPage(comparisonData));
                }
                else
                {
                    await DisplayAlert("Error", $"Upload failed with status code: {response.StatusCode}", "OK");
                    Console.WriteLine($"Upload failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to upload image: {ex.Message}", "OK");
                Console.WriteLine($"Error during upload: {ex.Message}");
            }
        }

    }



}


