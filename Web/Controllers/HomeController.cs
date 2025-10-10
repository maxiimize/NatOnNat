using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web.Models;

namespace Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IWeatherService _weatherService;

        public HomeController(
            ILogger<HomeController> logger,
            IProductService productService,
            IWeatherService weatherService)
        {
            _logger = logger;
            _productService = productService;
            _weatherService = weatherService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var viewModel = new HomeViewModel
                {
                    FavoriteProducts = await _productService.GetFavoriteProductsAsync(3),
                    NewestProducts = await _productService.GetNewestProductsAsync(3),
                    AllProducts = await _productService.GetAllProductsAsync(),
                    CurrentWeather = await _weatherService.GetCurrentWeatherAsync("Stockholm")
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading home page");
                return View(new HomeViewModel());
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
