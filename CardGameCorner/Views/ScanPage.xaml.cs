//using CardGameCorner.Services;
//using CardGameCorner.ViewModels;
//using CommunityToolkit.Maui.Views;
//using Microsoft.Maui.Controls;

//namespace CardGameCorner.Views
//{
//    public partial class ScanPage : ContentPage
//    {
//        private readonly IScanCardService _scanCardService;

//        public ScanPage()
//        {
//            InitializeComponent();
//            _scanCardService = new ScanCardService();
//        }

//        private async void OnCaptureClicked(object sender, EventArgs e)
//        {
//            try
//            {
//                var result = await cameraView.CaptureAsync();
//                if (result != null && result is IScreenshotResult screenshot)
//                {
//                    using var stream = await screenshot.OpenReadAsync();
//                    await ProcessCapturedImage(stream);
//                }
//            }
//            catch (Exception ex)
//            {
//                await DisplayAlert("Capture Error", ex.Message, "OK");
//            }
//        }

//        private async void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
//        {
//            if (e?.Media != null)
//            {
//                await ProcessCapturedImage(e.Media);
//            }
//        }

//        private async Task ProcessCapturedImage(Stream imageStream)
//        {
//            if (imageStream == null)
//            {
//                await DisplayAlert("Error", "Failed to capture image", "OK");
//                return;
//            }

//            try
//            {
//                // Convert stream to byte array
//                byte[] imageData;
//                using (var memoryStream = new MemoryStream())
//                {
//                    await imageStream.CopyToAsync(memoryStream);
//                    imageData = memoryStream.ToArray();
//                }

//                // Show preview
//                previewImage.Source = ImageSource.FromStream(() => new MemoryStream(imageData));
//                previewImage.IsVisible = true;

//                // Upload to API
//                var apiResponse = await _scanCardService.UploadImageAsync(imageData);
//                await DisplayAlert("Success", $"API Response: {apiResponse}", "OK");
//            }
//            catch (Exception ex)
//            {
//                await DisplayAlert("Processing Error", ex.Message, "OK");
//            }
//        }
//    }

//}

using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using System.Net.Http.Headers;

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
                    using (var stream = await photo.OpenReadAsync())
                    {
                        // Load the image into the Image control
                        // capturedImage.Source = ImageSource.FromStream(() => stream);
                        // capturedImage.IsVisible = true; // Make the image visible

                        // Reopen the stream to check the size (optional since we used it above)
                        var newStream = await photo.OpenReadAsync();
                        long imageSize = newStream.Length; // Get the size in bytes
                        Console.WriteLine($"Captured image size: {imageSize / 1024.0:F2} KB");

                        // Prepare the HTTP client and multipart content
                        using (var httpClient = new HttpClient())
                        using (var multipartContent = new MultipartFormDataContent())
                        {
                            // Create StreamContent from the captured image
                            newStream.Position = 0; // Reset stream position
                            var fileContent = new StreamContent(newStream);
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

                            // Add the file to the multipart content
                            multipartContent.Add(fileContent, "image", photo.FileName);

                            // Add additional form fields if needed
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
                            }
                            else
                            {
                                Console.WriteLine($"Upload failed: {response.StatusCode}");
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

                    //// Display the image in the Image control
                    //capturedImage.Source = ImageSource.FromStream(() => originalStream);
                    //capturedImage.IsVisible = true;

                    // Reset the stream position and clone it for the API
                    originalStream.Position = 0;
                    using var clonedStream = new MemoryStream();
                    await originalStream.CopyToAsync(clonedStream);
                    clonedStream.Position = 0; // Reset cloned stream

                    // Upload the cloned stream to the API
                    await UploadImageToApi(clonedStream);
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

                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Success", "Image uploaded successfully!", "OK");
                    Console.WriteLine($"Upload successful: {responseContent}");
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

