using Application.DTOs;

namespace Web.Models
{
    public class HomeViewModel
    {
        public IEnumerable<ProductDto> FavoriteProducts { get; set; } = new List<ProductDto>();
        public IEnumerable<ProductDto> NewestProducts { get; set; } = new List<ProductDto>();
        public IEnumerable<ProductDto> AllProducts { get; set; } = new List<ProductDto>();
        public WeatherDto? CurrentWeather { get; set; }
    }
}
