using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;

namespace CardGameCorner.Services
{
    public class ScanCardService:IScanCardService
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
            string game = "pokemon"; // Replace with the specific game you're using
            string apiKey = "0d66cf7894c3ed46592332829e6d467b"; // Replace with your actual API key
            string apiUrl = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";


            //_httpClient.DefaultRequestHeaders.Authorization =
            //    new AuthenticationHeaderValue("Bearer", apiKey);

            imageStream.Seek(0, SeekOrigin.Begin);

            using (var content = new MultipartFormDataContent())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await imageStream.CopyToAsync(memoryStream);
                    byte[] imageBytes = memoryStream.ToArray();

                    var imageContent = new ByteArrayContent(imageBytes);
                    imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");

                    content.Add(imageContent, "image", $"scannedimage.jpg");

                    var response = await _httpClient.PostAsync(apiUrl, content);

                    var responseContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("API Response:");
                    Console.WriteLine(responseContent);

                    if (response.IsSuccessStatusCode)
                    {
                        await Application.Current.MainPage.DisplayAlert("Upload Success", "Image uploaded successfully", "OK");
                    }
                    else
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        await Application.Current.MainPage.DisplayAlert("Upload Failed",
                            $"Status Code: {response.StatusCode}\nError: {errorContent}", "OK");
                    }
                }
            }
        }

       
    }

}


