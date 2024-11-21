
using CardGameCorner.Models;
using System.Net.Http.Json;
using System.Diagnostics;

namespace CardGameCorner.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ISecureStorage _secureStorage;
        private const string BaseUrl = "https://api.magiccorner.it/api/";

        public AuthService(ISecureStorage secureStorage)
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
            _secureStorage = secureStorage;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth", request);
                Debug.WriteLine($"Login response status: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
                    {
                        // Store the token as is, without any modification
                        await _secureStorage.SetAsync("jwt_token", loginResponse.Token);
                        Debug.WriteLine("Token stored successfully");
                        return loginResponse;
                    }
                    throw new Exception("Invalid token received from server");
                }

                var errorContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Login failed: {errorContent}");
                throw new Exception("Invalid credentials");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Login error: {ex.Message}");
                throw new Exception($"Login failed: {ex.Message}");
            }
        }

        public async Task RequestPasswordReset(string email)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("forgot-password", new { email });
                if (!response.IsSuccessStatusCode)
                {
                    throw new Exception("Failed to send reset link");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Password reset request failed: {ex.Message}");
            }
        }
    }
}