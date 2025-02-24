using System.Net.Http.Headers;
using System.Text.Json.Serialization;
using System.Text.Json;
using CardGameCorner.Services;
using ISecureStorage = CardGameCorner.Services.ISecureStorage;

public class OrderService : IOrderService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "https://api.magiccorner.it/api/";
    private readonly ISecureStorage _secureStorage;
    private readonly JsonSerializerOptions _jsonOptions;

    public OrderService(HttpClient httpClient, ISecureStorage secureStorage)
    {
        _httpClient = httpClient;
        _secureStorage = secureStorage;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<OrderDetail> GetOrderDetailAsync(int orderId)
    {
        try
        {
            var token = await _secureStorage.GetAsync("jwt_token");
            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedAccessException("No valid token found");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync($"{BaseUrl}mcBuylistMyOrder/{orderId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<OrderDetail>(content, _jsonOptions);

            if (result == null)
            {
                throw new Exception("Failed to deserialize order details");
            }

            return result;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to fetch order details: {ex.Message}", ex);
        }
    }
}