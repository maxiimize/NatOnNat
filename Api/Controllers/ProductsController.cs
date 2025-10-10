using Domain.Entities;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductRepository productRepository, ILogger<ProductsController> logger)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetAll()
        {
            try
            {
                var products = await _productRepository.GetAllAsync();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                return StatusCode(500, "An error occurred while fetching products");
            }
        }

        [HttpGet("favorites")]
        public async Task<ActionResult<IEnumerable<Product>>> GetFavorites([FromQuery] int count = 3)
        {
            try
            {
                if (count <= 0 || count > 10)
                {
                    return BadRequest("Count must be between 1 and 10");
                }

                var favorites = await _productRepository.GetFavoritesAsync(count);
                return Ok(favorites);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching favorite products");
                return StatusCode(500, "An error occurred while fetching favorite products");
            }
        }

        [HttpGet("newest")]
        public async Task<ActionResult<IEnumerable<Product>>> GetNewest([FromQuery] int count = 3)
        {
            try
            {
                if (count <= 0 || count > 10)
                {
                    return BadRequest("Count must be between 1 and 10");
                }

                var newest = await _productRepository.GetNewestAsync(count);
                return Ok(newest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching newest products");
                return StatusCode(500, "An error occurred while fetching newest products");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetById(int id)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(id);
                if (product == null)
                {
                    return NotFound($"Product with id {id} not found");
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product with id {ProductId}", id);
                return StatusCode(500, "An error occurred while fetching the product");
            }
        }
    }
}