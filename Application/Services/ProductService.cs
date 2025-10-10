using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

public class ProductService : IProductService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ProductService> _logger;

    public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
    {
        try
        {
            var resp = await _httpClient.GetAsync("api/products");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching all products from API");
            return new List<ProductDto>();
        }
    }

    public async Task<IEnumerable<ProductDto>> GetFavoriteProductsAsync(int count = 3)
    {
        try
        {
            var resp = await _httpClient.GetAsync($"api/products/favorites?count={count}");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching favorite products from API");
            return new List<ProductDto>();
        }
    }

    public async Task<IEnumerable<ProductDto>> GetNewestProductsAsync(int count = 3)
    {
        try
        {
            var resp = await _httpClient.GetAsync($"api/products/newest?count={count}");
            resp.EnsureSuccessStatusCode();
            var json = await resp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<ProductDto>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching newest products from API");
            return new List<ProductDto>();
        }
    }
}
