using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int StockQuantity { get; set; }
        public bool IsFavorite { get; set; }
        public DateTime CreatedDate { get; set; }
        public string FormattedPrice => $"{Price:N0} kr";
        public bool IsNew => CreatedDate >= DateTime.Now.AddDays(-7);
        public bool InStock => StockQuantity > 0;
    }
}
