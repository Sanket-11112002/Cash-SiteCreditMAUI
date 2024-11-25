using System.Net.Http.Headers;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Views;
using SkiaSharp;


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
        //private async void OnCaptureButtonClicked(object sender, EventArgs e)
        //{
        //    //try
        //    //{
        //    //    // Capture the image
        //    //    await cameraView.CaptureImage(CancellationToken.None);
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    await DisplayAlert("Capture Error", $"Failed to capture image: {ex.Message}", "OK");
        //    //}


        //    try
        //    {
        //        // Capture an image
        //        var photo = await MediaPicker.CapturePhotoAsync();

        //        if (photo != null)
        //        {
        //            // Open the photo as a stream
        //            using (var stream = await photo.OpenReadAsync())
        //            {
        //                // Clone the stream for display purposes
        //                var streamForDisplay = new MemoryStream();
        //                await stream.CopyToAsync(streamForDisplay);
        //                streamForDisplay.Position = 0; // Reset position for displaying

        //                // Load the image into the Image control
        //                capturedImage.Source = ImageSource.FromStream(() => streamForDisplay);
        //                capturedImage.IsVisible = true; // Make the image visible

        //                // Log the image size (optional)
        //                long imageSize = stream.Length;
        //                Console.WriteLine($"Captured image size: {imageSize / 1024.0:F2} KB");

        //                // Clone the original stream for uploading to the API
        //                stream.Position = 0; // Reset position for uploading
        //                using (var clonedStreamForUpload = new MemoryStream())
        //                {
        //                    await stream.CopyToAsync(clonedStreamForUpload);
        //                    clonedStreamForUpload.Position = 0; // Reset the position for upload

        //                    // Prepare the HTTP client and multipart content
        //                    using (var httpClient = new HttpClient())
        //                    using (var multipartContent = new MultipartFormDataContent())
        //                    {
        //                        // Create StreamContent from the captured image
        //                        var fileContent = new StreamContent(clonedStreamForUpload);
        //                        fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        //                        // Add the file to the multipart content
        //                        multipartContent.Add(fileContent, "image", photo.FileName);

        //                        // Add additional form fields if needed
        //                        string game = "pokemon";
        //                        string apiKey = "0d66cf7894c3ed46592332829e6d467b";
        //                        string url = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

        //                        // Send the request
        //                        var response = await httpClient.PostAsync(url, multipartContent);
        //                        var responseContent = await response.Content.ReadAsStringAsync();

        //                        // Handle the response
        //                        if (response.IsSuccessStatusCode)
        //                        {
        //                            Console.WriteLine($"Upload successful: {responseContent}");
        //                        }
        //                        else
        //                        {
        //                            Console.WriteLine($"Upload failed: {response.StatusCode}");
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Console.WriteLine("No image captured.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"An error occurred: {ex.Message}");
        //    }
        //}

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
                        // Resize and compress the image to be under 100 KB
                        var compressedImageStream = await CompressImageAsync(stream, 100 * 1024); // 100 KB

                        // Load the compressed image into the Image control for display
                        capturedImage.Source = ImageSource.FromStream(() => compressedImageStream);
                        capturedImage.IsVisible = true;

                        // Log the image size (optional)
                        long imageSize = compressedImageStream.Length;
                        Console.WriteLine($"Captured and compressed image size: {imageSize / 1024.0:F2} KB");
                        var size = imageSize / 1024.0;
                        // Reset the position of the stream for upload
                        compressedImageStream.Position = 0;

                        // Prepare the HTTP client and multipart content
                        using (var httpClient = new HttpClient())
                        using (var multipartContent = new MultipartFormDataContent())
                        {
                            // Create StreamContent from the compressed image
                            var fileContent = new StreamContent(compressedImageStream);
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
                                await DisplayAlert("Success", "Image uploaded successfully!", "OK");
                                Console.WriteLine($"Upload successful: {responseContent}");
                            }
                            else
                            {
                                await DisplayAlert("Error", $"Upload failed with status code: {response.StatusCode}", "OK");
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

        // Method to compress the image stream and ensure it is under the maxSize
        private async Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        {
            // Load the image into a SKBitmap
            using (var skiaImage = SKBitmap.Decode(inputStream))
            {
                // Resize the image to reduce its dimensions
                int width = 800; // Set the width (you can adjust as needed)
                int height = (int)((float)skiaImage.Height * width / skiaImage.Width);

                var resizedImage = skiaImage.Resize(new SKImageInfo(width, height), SKFilterQuality.Medium);

                // Compress the resized image into a MemoryStream
                var memoryStream = new MemoryStream();
                using (var skImage = SKImage.FromBitmap(resizedImage))
                {
                    var skData = skImage.Encode(SKEncodedImageFormat.Jpeg, 85); // 85% quality
                    skData.SaveTo(memoryStream);
                }

                memoryStream.Position = 0; // Reset the stream position to the beginning
                                           // Check if the image size is under maxSize and adjust quality if necessary
                while (memoryStream.Length > maxSize)
                {
                    memoryStream.SetLength(0); // Clear the memory stream
                    using (var skImage = SKImage.FromBitmap(resizedImage))
                    {
                        var skData = skImage.Encode(SKEncodedImageFormat.Jpeg, 80); // Lower quality if still too large
                        skData.SaveTo(memoryStream);
                    }
                    memoryStream.Position = 0; // Reset the stream position to the beginning
                }

                return memoryStream;
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

                    // Clone the stream for image display
                    var clonedStreamForDisplay = new MemoryStream();
                    await originalStream.CopyToAsync(clonedStreamForDisplay);
                    clonedStreamForDisplay.Position = 0;  // Reset cloned stream position for display

                    // Display the image in the Image control
                    capturedImage.Source = ImageSource.FromStream(() => clonedStreamForDisplay);
                    capturedImage.IsVisible = true;

                    // Reset the position and clone again for the API upload
                    originalStream.Position = 0; // Reset the position of the original stream before uploading
                    using var clonedStreamForUpload = new MemoryStream();
                    await originalStream.CopyToAsync(clonedStreamForUpload);
                    clonedStreamForUpload.Position = 0;  // Reset cloned stream position for upload

                    // Upload the cloned stream to the API
                    await UploadImageToApi(clonedStreamForUpload);
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


