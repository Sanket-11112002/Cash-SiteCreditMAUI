﻿//using System;
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
using System.Text.Json;
using System.Threading.Tasks;

using CardGameCorner.Models;
using CardGameCorner.ViewModels;
using CommunityToolkit.Maui.Views;

namespace CardGameCorner.Services
{
    public class ScanCardService : IScanCardService
    {
        private readonly HttpClient _httpClient;
        public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
        public ScanCardService()
        {
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(30)
            };
            _httpClient.DefaultRequestHeaders.Authorization =
           new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f");

        }

        //public async Task UploadImageAsync(Stream imageStream)
        //{
        //    try
        //    {
        //        // Validate stream
        //        if (imageStream == null || !imageStream.CanRead)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error",
        //                "Invalid image stream. Please try capturing the image again.", "OK");
        //            return;
        //        }

        //        // Verify stream has content
        //        if (imageStream.Length == 0)
        //        {
        //            await Application.Current.MainPage.DisplayAlert("Error",
        //                "Empty image stream. Please try capturing the image again.", "OK");
        //            return;
        //        }

        //        string game = "pokemon";
        //        string apiKey = "0d66cf7894c3ed46592332829e6d467b";
        //        string apiUrl = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";

        //        // Reset stream position
        //        imageStream.Position = 0;

        //        using (var memoryStream = new MemoryStream())
        //        {
        //            await imageStream.CopyToAsync(memoryStream);

        //            // Verify the copied data
        //            if (memoryStream.Length == 0)
        //            {
        //                await Application.Current.MainPage.DisplayAlert("Error",
        //                    "Failed to process image data. Please try again.", "OK");
        //                return;
        //            }

        //            byte[] imageBytes = memoryStream.ToArray();

        //            using (var content = new MultipartFormDataContent())
        //            {
        //                var imageContent = new ByteArrayContent(imageBytes);
        //                imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("image/jpeg");
        //                content.Add(imageContent, "image", "scannedimage.jpg");

        //                // Log request details for debugging
        //                Console.WriteLine($"Sending request to: {apiUrl}");
        //                Console.WriteLine($"Image size: {imageBytes.Length} bytes");

        //                var response = await _httpClient.PostAsync(apiUrl, content);
        //                var responseContent = await response.Content.ReadAsStringAsync();

        //                Console.WriteLine("API Response:");
        //                Console.WriteLine(responseContent);

        //                if (response.IsSuccessStatusCode)
        //                {
        //                    await Application.Current.MainPage.DisplayAlert("Success",
        //                        "Image uploaded successfully", "OK");
        //                }
        //                else
        //                {
        //                    var errorMessage = $"Status Code: {response.StatusCode}\nError: {responseContent}";
        //                    Console.WriteLine($"Error details: {errorMessage}");
        //                    await Application.Current.MainPage.DisplayAlert("Upload Failed",
        //                        errorMessage, "OK");
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Exception details: {ex}");
        //        await Application.Current.MainPage.DisplayAlert("Error",
        //            $"An error occurred: {ex.Message}", "OK");
        //    }
        //}

        //public async Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        //{
        //    // Move image compression logic from the ViewModel here
        //    return await ScanCardViewModel.CompressImageAsync(inputStream, maxSize);
        //}


        public async Task<ApiResponse_Card> UploadImageAsync(Stream imageStream)
        {
            using var httpClient = new HttpClient();
            using var multipartContent = new MultipartFormDataContent();
            var fileContent = new StreamContent(imageStream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

            multipartContent.Add(fileContent, "image", "uploaded_image.jpg");

            string game = GlobalSettings.SelectedGame;
            string apiKey = "0d66cf7894c3ed46592332829e6d467b";
            string url = $"https://api2.magic-sorter.com/image/{game}?mess_detector=0&upside=0&foil=0&lang=en&set_type=2&set[]=&api_key={apiKey}";
            try
            {
                var response = await httpClient.PostAsync(url, multipartContent);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                    Console.WriteLine($"Upload successful: {responseContent}");

                    // Deserialize the response into ApiResponse_Card

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse_Card>(responseContent, options);
                    Console.WriteLine($"Card Title: {apiResponse.Result.Title}");

                    return JsonSerializer.Deserialize<ApiResponse_Card>(responseContent, options);

                }
                else
                {
                    //throw new Exception($"Upload failed with status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception e)
            {
                return null;
            }

        }



        public async Task<List<CardSearchResponseViewModel>> SearchCardAsync(CardSearchRequest cardRequest)
        {
            string cardSearchUrl = "https://api.magiccorner.it/api/cardsearch";
            var cardRequestContent = new StringContent(
                JsonSerializer.Serialize(cardRequest),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(cardSearchUrl, cardRequestContent);
            var responseContent = await response.Content.ReadAsStringAsync();


            if (response.IsSuccessStatusCode)
            {
                try
                {
                    Console.WriteLine($"Response Content: {responseContent}");

                    var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

                    // Check if the response is a list or single object
                    var responseWrapper = JsonSerializer.Deserialize<CardSearchResponseViewModel>(responseContent, options);

                    // Wrap the response in a list to return as a List<CardSearchResponseViewModel>
                    return new List<CardSearchResponseViewModel> { responseWrapper };
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"JSON deserialization failed: {ex.Message}");
                    throw;
                }
            }
            else
            {
                throw new Exception($"Card search failed: {response.StatusCode}. {responseContent}");
            }
        }

        //    public async Task<List<CardSearchResponseViewModel>> SearchCardAsync(CardSearchRequest cardRequest)
        //    {
        //        // Hardcoded response JSON
        //        var responseContent = @"
        //{
        //    ""Products"": [
        //        {
        //            ""Id"": ""IMA006"",
        //            ""Game"": ""magic"",
        //            ""Model"": ""Angelo della Misericordia"",
        //            ""ModelEn"": ""Angel of Mercy"",
        //            ""ModelSeo"": ""angelo-della-misericordia"",
        //            ""ModelSeoEn"": ""angel-of-mercy"",
        //            ""Image"": ""/prodotti/1/1115/IMA006.jpg"",
        //            ""Color"": ""a"",
        //            ""ColorLong"": ""White"",
        //            ""Rarity"": ""Common"",
        //            ""CategorySeo"": ""iconic-masters"",
        //            ""Category"": ""Iconic Masters"",
        //            ""IdCategory"": 1115,
        //            ""Icon"": ""IMA"",
        //            ""Quantity"": 7,
        //            ""Variants"": [
        //                {
        //                    ""IdProduct"": 240683,
        //                    ""FirstEdition"": """",
        //                    ""Condition"": ""NM/M"",
        //                    ""Foil"": ""Foil"",
        //                    ""Language"": ""English"",
        //                    ""Price"": 0.3000,
        //                    ""Quantity"": 7
        //                }
        //            ],
        //            ""MinPrice"": 0.16,
        //            ""MaxPrice"": 0.3,
        //            ""BuyListLock"": false,
        //            ""isFoil"": false,
        //            ""CategorySortOrder"": 337,
        //            ""SetCode"": ""IMA""
        //        },
        //        {
        //            ""Id"": ""IMA007"",
        //            ""Game"": ""magic"",
        //            ""Model"": ""Second Product"",
        //            ""ModelEn"": ""Second Product"",
        //            ""ModelSeo"": ""second-product"",
        //            ""ModelSeoEn"": ""second-product"",
        //            ""Image"": ""/prodotti/1/1115/IMA007.jpg"",
        //            ""Color"": ""b"",
        //            ""ColorLong"": ""Black"",
        //            ""Rarity"": ""Rare"",
        //            ""CategorySeo"": ""iconic-masters-rare"",
        //            ""Category"": ""Iconic Masters Rare"",
        //            ""IdCategory"": 1116,
        //            ""Icon"": ""IMA"",
        //            ""Quantity"": 3,
        //            ""Variants"": [
        //                {
        //                    ""IdProduct"": 240684,
        //                    ""FirstEdition"": """",
        //                    ""Condition"": ""NM/M"",
        //                    ""Foil"": ""Foil"",
        //                    ""Language"": ""English"",
        //                    ""Price"": 0.5000,
        //                    ""Quantity"": 3
        //                }
        //            ],
        //            ""MinPrice"": 0.4,
        //            ""MaxPrice"": 0.5,
        //            ""BuyListLock"": false,
        //            ""isFoil"": true,
        //            ""CategorySortOrder"": 338,
        //            ""SetCode"": ""IMA""
        //        }
        //    ],
        //    ""Total"": 2
        //}";

        //        try
        //        {
        //            Console.WriteLine($"Response Content: {responseContent}");

        //            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };




        //            // Deserialize into the wrapper class
        //            // Deserialize into the wrapper class
        //            var responseWrapper = JsonSerializer.Deserialize<CardSearchResponseViewModel>(responseContent, options);

        //            // Wrap the response in a list to return as a List<CardSearchResponseViewModel>
        //            return new List<CardSearchResponseViewModel> { responseWrapper };
        //            // Return the list of products
        //        }
        //        catch (JsonException ex)
        //        {
        //            Console.WriteLine($"JSON deserialization failed: {ex.Message}");
        //            throw;
        //        }
        //    }


        public Task<MemoryStream> CompressImageAsync(Stream inputStream, long maxSize)
        {
            throw new NotImplementedException();
        }
        public async Task<BuyListPriceResponse> FetchBuyListPriceAsync(cardDetailRequest request)
        {
            try
            {
                // Define the API URL
                string url = "https://api.magiccorner.it/api/mcgetbuylistprice";


                var cardRequestContent = new StringContent(
               JsonSerializer.Serialize(request),
               Encoding.UTF8,
               "application/json"
           );
                var response = await _httpClient.PostAsync(url, cardRequestContent);
                var responseContent = await response.Content.ReadAsStringAsync();



                // Make the GET request


                // Ensure successful status code (200-299)

                // Read the response content as string


                var data = new BuyListPriceResponse();

                // Deserialize the JSON content into a C# object
                data = JsonSerializer.Deserialize<BuyListPriceResponse>(responseContent);

                // Process the data (for example, print it)
                if (data != null)
                {
                    Console.WriteLine($"Cash Price: {data.buylist}");
                    Console.WriteLine($"Site Credit: {data.SiteCredit}");
                }
                return data;
            }
            catch (HttpRequestException e)
            {
                // Handle HTTP request exceptions
                return null;
                Console.WriteLine($"Request error: {e.Message}");
            }
            catch (Exception e)
            {
                return null;
                // Handle other exceptions
                Console.WriteLine($"An error occurred: {e.Message}");
            }
        }

    }
}