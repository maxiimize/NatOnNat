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
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WeatherService> _logger;
        private readonly string _apiKey;

        public WeatherService(HttpClient httpClient, ILogger<WeatherService> logger, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _logger = logger;
            _apiKey = configuration["WeatherApi:ApiKey"] ?? "YOUR_API_KEY_HERE";
        }

        public async Task<WeatherDto?> GetCurrentWeatherAsync(string city = "Stockholm")
        {
            try
            {
                var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={_apiKey}&units=metric&lang=sv";
                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Weather API returned {StatusCode}", response.StatusCode);
                    // Return fallback weather data for demo purposes
                    return new WeatherDto
                    {
                        Location = city,
                        Temperature = 15,
                        Description = "Molnigt",
                        Icon = "☁️",
                        FeelsLike = 14,
                        Humidity = 65,
                        WindSpeed = 3.5
                    };
                }

                var json = await response.Content.ReadAsStringAsync();
                var weatherData = JsonSerializer.Deserialize<JsonElement>(json);

                return new WeatherDto
                {
                    Location = weatherData.GetProperty("name").GetString() ?? city,
                    Temperature = weatherData.GetProperty("main").GetProperty("temp").GetDouble(),
                    Description = weatherData.GetProperty("weather")[0].GetProperty("description").GetString() ?? "",
                    Icon = GetWeatherIcon(weatherData.GetProperty("weather")[0].GetProperty("icon").GetString() ?? ""),
                    FeelsLike = weatherData.GetProperty("main").GetProperty("feels_like").GetDouble(),
                    Humidity = weatherData.GetProperty("main").GetProperty("humidity").GetInt32(),
                    WindSpeed = weatherData.GetProperty("wind").GetProperty("speed").GetDouble()
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching weather data");
                // Return fallback data
                return new WeatherDto
                {
                    Location = city,
                    Temperature = 15,
                    Description = "Molnigt",
                    Icon = "☁️",
                    FeelsLike = 14,
                    Humidity = 65,
                    WindSpeed = 3.5
                };
            }
        }

        private string GetWeatherIcon(string iconCode)
        {
            return iconCode switch
            {
                "01d" => "☀️",
                "01n" => "🌙",
                "02d" or "02n" => "⛅",
                "03d" or "03n" => "☁️",
                "04d" or "04n" => "☁️",
                "09d" or "09n" => "🌧️",
                "10d" or "10n" => "🌦️",
                "11d" or "11n" => "⛈️",
                "13d" or "13n" => "❄️",
                "50d" or "50n" => "🌫️",
                _ => "☁️"
            };
        }
    }
