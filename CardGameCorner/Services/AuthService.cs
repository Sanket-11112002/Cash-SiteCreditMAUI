using CardGameCorner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace CardGameCorner.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "https://api.magiccorner.it/api/";

        public AuthService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("auth", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LoginResponse>();
                }
                throw new Exception("Invalid credentials");
            }
            catch (Exception ex)
            {
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
