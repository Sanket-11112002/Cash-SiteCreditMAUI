using System.Diagnostics;
using System.Text.Json;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using CardGameCorner.Constants;
using CardGameCorner.Models;

namespace CardGameCorner.Services
{
    public class GameService : IGameService
    {
        private readonly HttpClient _httpClient;
        private readonly ISecureStorage _secureStorage;
        private readonly JsonSerializerOptions _jsonOptions;

        public GameService(ISecureStorage secureStorage)
        {
            _secureStorage = secureStorage;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://api.magiccorner.it/")  // Make sure this matches your API base URL
            };
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        private async Task<bool> SetAuthenticationHeader()
        {
            try
            {
                var token = "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f";
                Debug.WriteLine($"Retrieved token: {token}");

                if (!string.IsNullOrEmpty(token))
                {
                    // Clear existing headers
                    _httpClient.DefaultRequestHeaders.Authorization = null;



                    // Set the authorization header with the exact format
                    _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

                    Debug.WriteLine($"Set Authorization header: Bearer {token}");
                    return true;
                }

                Debug.WriteLine("No token found in secure storage");
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error setting auth header: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Game>> GetGamesAsync()
        {
            try
            {
                var hasToken = await SetAuthenticationHeader();
                if (!hasToken)
                {
                    Debug.WriteLine("No valid token available");
                    throw new UnauthorizedAccessException("No valid authentication token found.");
                }

                // Print all headers for debugging
                foreach (var header in _httpClient.DefaultRequestHeaders)
                {
                    Debug.WriteLine($"Header: {header.Key}: {string.Join(", ", header.Value)}");
                }

                var response = await _httpClient.GetAsync("api/buylistgames");
                Debug.WriteLine($"Response Status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Response Content: {content}");
                    var games = JsonSerializer.Deserialize<List<Game>>(content, _jsonOptions);

                    return games ?? new List<Game>();
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Unauthorized error: {errorContent}");
                    throw new UnauthorizedAccessException("Unauthorized access. Please check your credentials.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error Response: {errorContent}");
                    throw new HttpRequestException($"API returned {response.StatusCode}: {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetGamesAsync: {ex.Message}");
                throw;
            }
        }
    }
}