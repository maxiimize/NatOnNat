using Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<IEnumerable<ProductDto>> GetFavoriteProductsAsync(int count = 3);
        Task<IEnumerable<ProductDto>> GetNewestProductsAsync(int count = 3);
    }
}
