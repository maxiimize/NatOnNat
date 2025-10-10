using Application.DTOs;
using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Application.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;
        private readonly string _apiBaseUrl;

        public ProductService(HttpClient httpClient, ILogger<ProductService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7257/api";
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/products");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var products = JsonSerializer.Deserialize<List<ProductDto>>(json, options) ?? new List<ProductDto>();

                return products.OrderBy(p => p.Name);
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/products/favorites?count={count}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<ProductDto>>(json, options) ?? new List<ProductDto>();
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
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}/products/newest?count={count}");
                response.EnsureSuccessStatusCode();

                var json = await response.Content.ReadAsStringAsync();
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<List<ProductDto>>(json, options) ?? new List<ProductDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching newest products from API");
                return new List<ProductDto>();
            }
        }
    }
}
