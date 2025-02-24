using CardGameCorner.Models;
using CardGameCorner.Services;
using CardGameCorner.ViewModels;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class SearchService
{
    private readonly HttpClient _httpClient;
    private const int DefaultPageSize = 20;
    public GlobalSettingsService GlobalSettings => GlobalSettingsService.Current;
    public SearchService()
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", "0d1bb073-9dfb-4c6d-a1c0-1e8f7d5d8e9f");
    }

    // public async Task<List<Product>> SearchProductsAsync(string query)
    public async Task<List<Product>> SearchProductsAsync(string query, SearchFilters filters = null)
    {
        // Filters code
        filters ??= new SearchFilters();

        var requestBody = new
        {
            q = query?.Trim() ?? "", // Trim any whitespace and handle null
            game = GlobalSettings.SelectedGame,
            edition = filters.Edition ?? "",
            rarity = filters.Rarity ?? "",
            color = filters.Colors ?? "",
            firstedition = filters.FirstEdition ?? "",
            foil = "",
            language = filters.Language ?? "",
            page = 1,
            pageSize = DefaultPageSize,
            sort = 5,
            isBuyList = true,
            onlyHotBuyList = filters.HotList?.Equals("Hot BuyList Only", StringComparison.OrdinalIgnoreCase) == true,
            onlyAvailable = false
        };

        string json = JsonSerializer.Serialize(requestBody, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase // This ensures properties are serialized in camelCase
        });

        var content = new StringContent(json, Encoding.UTF8, "application/json");

        try
        {
            var response = await _httpClient.PostAsync("https://api.magiccorner.it/api/mcadvsearch", content);

            // For debugging - log the request
            Console.WriteLine($"Request Body: {json}");

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();

                // For debugging - log the response
                Console.WriteLine($"Response: {result}");

                var apiResponse = JsonSerializer.Deserialize<ApiResponse>(result, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (apiResponse?.Products != null)
                {
                    return apiResponse.Products;
                }
                else
                {
                    Console.WriteLine("No products found in response");
                    return new List<Product>();
                }
            }
            else
            {
                string errorDetails = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"API Error: {errorDetails}");
                throw new HttpRequestException($"Error {response.StatusCode}: {response.ReasonPhrase}. Details: {errorDetails}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception occurred: {ex}");
            throw new Exception("An error occurred while searching for products. Please try again later.", ex);
        }
    }
}