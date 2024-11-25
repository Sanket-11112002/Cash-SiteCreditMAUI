//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;
//using CommunityToolkit.Maui.Views;

//namespace CardGameCorner.Services
//{
//    public class ScanCardService : IScanCardService
//    {
//        private readonly HttpClient _httpClient;
//        private const string ApiKey = "0d66cf7894c3ed46592332829e6d467b";

//        public ScanCardService()
//        {
//            _httpClient = new HttpClient
//            {
//                Timeout = TimeSpan.FromSeconds(30)
//            };
//        }

//        public async Task<string> UploadImageAsync(byte[] imageData)
//        {
//            if (imageData == null || imageData.Length == 0)
//                return await UploadInvalidImageAsync("Empty image data");

//            string game = "pokemon";
//            string apiUrl = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={ApiKey}";

//            try
//            {
//                using var content = new MultipartFormDataContent();
//                var imageContent = new ByteArrayContent(imageData);
//                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
//                content.Add(imageContent, "image", "scannedimage.jpg");

//                var response = await _httpClient.PostAsync(apiUrl, content);
//                var responseContent = await response.Content.ReadAsStringAsync();

//                if (!response.IsSuccessStatusCode)
//                {
//                    return await UploadInvalidImageAsync($"API request failed: {response.StatusCode}");
//                }

//                return responseContent;
//            }
//            catch (Exception ex)
//            {
//                return await UploadInvalidImageAsync($"Image upload error: {ex.Message}");
//            }
//        }

//        public async Task<string> UploadInvalidImageAsync(string errorMessage)
//        {
//            string game = "pokemon";
//            string apiUrl = $"https://api2.magic-sorter.com/image/{game}/error?api_key={ApiKey}";

//            try
//            {
//                var content = new StringContent(
//                    JsonSerializer.Serialize(new { error = errorMessage }),
//                    Encoding.UTF8,
//                    "application/json"
//                );

//                var response = await _httpClient.PostAsync(apiUrl, content);
//                return await response.Content.ReadAsStringAsync();
//            }
//            catch
//            {
//                // Fallback error logging
//                return $"Failed to log error: {errorMessage}";
//            }
//        }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace CardGameCorner.Services
{
    public class ScanCardService : IScanCardService
    {
        private readonly HttpClient _httpClient;

        public ScanCardService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
        }

        public async Task UploadImageAsync(Stream imageStream)
        {
            try
            {
                // Validate stream
                if (imageStream == null || !imageStream.CanRead)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "Invalid image stream. Please try capturing the image again.", "OK");
                    return;
                }

                // Verify stream has content
                if (imageStream.Length == 0)
                {
                    await Application.Current.MainPage.DisplayAlert("Error",
                        "Empty image stream. Please try capturing the image again.", "OK");
                    return;
                }

                string game = "pokemon";
                string apiKey = "0d66cf7894c3ed46592332829e6d467b";
                string apiUrl = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

                // Reset stream position
                imageStream.Position = 0;

                using (var memoryStream = new MemoryStream())
                {
                    await imageStream.CopyToAsync(memoryStream);

                    // Verify the copied data
                    if (memoryStream.Length == 0)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error",
                            "Failed to process image data. Please try again.", "OK");
                        return;
                    }

                    byte[] imageBytes = memoryStream.ToArray();

                    using (var content = new MultipartFormDataContent())
                    {
                        var imageContent = new ByteArrayContent(imageBytes);
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
                        content.Add(imageContent, "image", "scannedimage.jpg");

                        // Log request details for debugging
                        Console.WriteLine($"Sending request to: {apiUrl}");
                        Console.WriteLine($"Image size: {imageBytes.Length} bytes");

                        var response = await _httpClient.PostAsync(apiUrl, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        Console.WriteLine("API Response:");
                        Console.WriteLine(responseContent);

                        if (response.IsSuccessStatusCode)
                        {
                            //await Application.Current.MainPage.DisplayAlert("Success",
                            //    "Image uploaded successfully", "OK");
                        }
                        else
                        {
                            //var errorMessage = $"Status Code: {response.StatusCode}\nError: {responseContent}";
                            //Console.WriteLine($"Error details: {errorMessage}");
                            //await Application.Current.MainPage.DisplayAlert("Upload Failed",
                            //    errorMessage, "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception details: {ex}");
                await Application.Current.MainPage.DisplayAlert("Error",
                    $"An error occurred: {ex.Message}", "OK");
            }
        }
    }
}