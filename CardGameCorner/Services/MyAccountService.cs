//using CardGameCorner.Models;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Text.Json;
//using System.Threading.Tasks;

//namespace CardGameCorner.Services
//{
//    public class MyAccountService : IMyAccountService
//    {
//        private readonly HttpClient _httpClient;
//        private readonly ISecureStorage _secureStorage;
//        private const string BaseUrl = "https://api.magiccorner.it/api/";

//        public MyAccountService(ISecureStorage secureStorage)
//        {
//            _httpClient = new HttpClient();
//            _httpClient.BaseAddress = new Uri(BaseUrl);
//            _secureStorage = secureStorage;
//        }

//        public async Task<UserProfile> GetUserProfileAsync()
//        {
//            var token = await _secureStorage.GetAsync("jwt_token");
//            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

//            var response = await _httpClient.GetAsync("myAccount");
//            response.EnsureSuccessStatusCode();

//            //var json = @"{
//            //                    ""userName"": ""test@m.com"",
//            //                    ""email"": ""test@m.com"",
//            //                    ""phoneNumber"": null,
//            //                    ""name"": ""sss"",
//            //                    ""lastName"": ""ddd"",
//            //                    ""company"": ""sdd"",
//            //                    ""vatNumber"": ""123"",
//            //                    ""fiscalCode"": ""12th d SW"",
//            //                    ""address"": ""wdd"",
//            //                    ""zip"": ""234"",
//            //                    ""province"": ""xxx"",
//            //                    ""city"": ""xxx"",
//            //                    ""country"": ""ssd"",
//            //                    ""phone"": ""2345678990"",
//            //                    ""optional1"": null,
//            //                    ""optional2"": null,
//            //                    ""optional3"": null,
//            //                    ""optional4"": null,
//            //                    ""deliveryAddresses"": [],
//            //                    ""orders"": null
//            //                }";

//            //var userProfile = JsonSerializer.Deserialize<UserProfile>(json, new JsonSerializerOptions
//            //{
//            //    PropertyNameCaseInsensitive = true
//            //});

//            //Console.WriteLine($"UserName: {userProfile.UserName}, Email: {userProfile.Email}");


//            var content = await response.Content.ReadAsStringAsync();
//            return JsonSerializer.Deserialize<UserProfile>(content, new JsonSerializerOptions
//            {
//                PropertyNameCaseInsensitive = true
//            });


//        }

//        public async Task<bool> UpdateUserProfileAsync(UserProfile profile)
//        {
//            var token = await _secureStorage.GetAsync("jwt_token");
//            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

//            var json = JsonSerializer.Serialize(profile);
//            var content = new StringContent(json, Encoding.UTF8, "application/json");

//            var response = await _httpClient.PutAsync("myAccount", content);
//            return response.IsSuccessStatusCode;
//        }
//    }
//}

using CardGameCorner.Models;
using System;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public class MyAccountService : IMyAccountService
    {
        private readonly HttpClient _httpClient;
        private readonly ISecureStorage _secureStorage;
        private const string BaseUrl = "https://api.magiccorner.it/api/";

        public MyAccountService(ISecureStorage secureStorage)
        {
            _httpClient = new HttpClient();
            _httpClient.BaseAddress = new Uri(BaseUrl);
            _secureStorage = secureStorage;
        }

        public async Task<UserProfile> GetUserProfileAsync()
        {
            var token = await _secureStorage.GetAsync("jwt_token");

            // If no token is found, throw an exception
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No authentication token found.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var response = await _httpClient.GetAsync("myAccount");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserProfile>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }
            catch (HttpRequestException)
            {
                // Clear the token if the request fails
                await _secureStorage.RemoveAsync("jwt_token");
                throw;
            }
        }

        public async Task<bool> UpdateUserProfileAsync(UserProfile profile)
        {
            var token = await _secureStorage.GetAsync("jwt_token");

            // If no token is found, throw an exception
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No authentication token found.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            try
            {
                var json = JsonSerializer.Serialize(profile);
                Console.WriteLine("Payload: " + json);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("myaccount/update", content);

                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                // Clear the token if the request fails
                await _secureStorage.RemoveAsync("jwt_token");
                throw;
            }
        }
    }
}