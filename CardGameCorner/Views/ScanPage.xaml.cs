using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using System.IO;

namespace CardGameCorner.Views
{
    public partial class ScanPage : ContentPage
    {
        private readonly HttpClient _httpClient;

        public ScanPage()
        {
            InitializeComponent();
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        private async void OnMediaCaptured(object sender, MediaCapturedEventArgs e)
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                try
                {
                    if (e.Media != null)
                    {
                        // Display the captured image
                        capturedImage.Source = ImageSource.FromStream(() => e.Media);
                        capturedImage.IsVisible = true;

                        // Upload the image
                        await UploadImageToApi(e.Media);
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"Failed to process captured image: {ex.Message}", "OK");
                }
            });
        }

        private async Task UploadImageToApi(Stream imageStream)
        {
            try
            {
                // Prepare the API URL with parameters
                string game = "pokemon"; // Replace with the specific game you're using
                string apiKey = "0d66cf7894c3ed46592332829e6d467b"; // Replace with your actual API key
                string apiUrl = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

                // Reset the stream position
                imageStream.Seek(0, SeekOrigin.Begin);

                // Prepare multipart form data
                using (var content = new MultipartFormDataContent())
                {
                    // Convert stream to byte array
                    using (var memoryStream = new MemoryStream())
                    {

                        await imageStream.CopyToAsync(memoryStream);
                        byte[] imageBytes = memoryStream.ToArray();

                        var imageContent = new ByteArrayContent(imageBytes);
                        imageContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/jpeg");

                        content.Add(imageContent, "image", "pokemon_card.jpg");


                        // Send POST request
                        var response = await _httpClient.PostAsync(apiUrl, content);

                        // Check response
                        if (response.IsSuccessStatusCode)
                        {
                            // Read and parse the response
                            var responseContent = await response.Content.ReadAsStringAsync();
                            await DisplayAlert("Upload Success", "Image uploaded successfully", "OK");

                            // Optionally, process the response
                            // var result = JsonSerializer.Deserialize<YourResponseType>(responseContent);
                        }
                        else
                        {
                            // Read error response
                            var errorContent = await response.Content.ReadAsStringAsync();
                            await DisplayAlert("Upload Failed",
                                $"Status Code: {response.StatusCode}\nError: {errorContent}",
                                "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Upload Error", $"Failed to upload image: {ex.Message}", "OK");
            }
        }

        private async void OnCaptureButtonClicked(object sender, EventArgs e)
        {
            try
            {
                // Capture the image
                await cameraView.CaptureImage(CancellationToken.None);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Capture Error", $"Failed to capture image: {ex.Message}", "OK");
            }
        }

        //private async void OnCaptureButtonClicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        // Capture an image
        //        var photo = await MediaPicker.CapturePhotoAsync();

        //        if (photo != null)
        //        {
        //            // Open the photo as a stream
        //            using (var stream = await photo.OpenReadAsync())
        //            {
        //                // Prepare the HTTP client and multipart content
        //                using (var httpClient = new HttpClient())
        //                using (var multipartContent = new MultipartFormDataContent())
        //                {
        //                    // Create StreamContent from the captured image
        //                    var fileContent = new StreamContent(stream)
        //;
        //                    fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        //                    // Add the file to the multipart content
        //                    multipartContent.Add(fileContent, "image", photo.FileName);

        //                    // Set the API endpoint
        //                    string game = "pokemon"; // Replace with the specific game you're using
        //                    string apiKey = "0d66cf7894c3ed46592332829e6d467b"; // Replace with your actual API key
        //                    string url = $"https://api2.magic-sorter.com/image/pokemon?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

        //                    // Send the request
        //                    var response = await httpClient.PostAsync(url, multipartContent);

        //                    // Log the server response
        //                    var responseContent = await response.Content.ReadAsStringAsync();

        //                    if (response.IsSuccessStatusCode)
        //                    {
        //                        Console.WriteLine($"Upload successful: {responseContent}");
        //                    }
        //                    else
        //                    {
        //                        Console.WriteLine($"Upload failed: {response.StatusCode}");
        //                        Console.WriteLine($"Server response: {responseContent}");
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
    }
}