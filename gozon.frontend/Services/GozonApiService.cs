using System.Text.Json;
using gozon.frontend.Models;

namespace gozon.frontend.Services;

public class GozonApiService(HttpClient httpClient, IConfiguration config)
{
    private readonly string _gatewayUrl = config["GatewayUrl"] ?? "http://gateway:8080";

    public async Task<decimal> GetBalanceAsync(Guid userId)
    {
        try
        {
            var response = await httpClient.GetFromJsonAsync<JsonElement>($"{_gatewayUrl}/api/payments/balance/{userId}");
            return response.GetProperty("balance").GetDecimal();
        }
        catch
        {
            return 0;
        }
    }
    
    public async Task<List<OrderViewModel>> GetOrdersAsync(Guid userId)
    {
        try 
        {
            return await httpClient.GetFromJsonAsync<List<OrderViewModel>>($"{_gatewayUrl}/api/orders/user/{userId}") 
                   ?? new List<OrderViewModel>();
        }
        catch
        {
            return new List<OrderViewModel>();
        }
    }

    public async Task CreateAccountAsync(Guid userId)
    {
        await httpClient.PostAsJsonAsync($"{_gatewayUrl}/api/payments/account", new { userId });
    }

    public async Task TopUpAsync(Guid userId, decimal amount)
    {
        await httpClient.PostAsJsonAsync($"{_gatewayUrl}/api/payments/topup", new { userId, amount });
    }

    public async Task CreateOrderAsync(Guid userId, decimal amount, string description)
    {
        await httpClient.PostAsJsonAsync($"{_gatewayUrl}/api/orders", new { userId, amount, description });
    }
}