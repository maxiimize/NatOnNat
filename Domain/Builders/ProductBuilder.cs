using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Builders
{
    public class ProductBuilder : IProductBuilder
    {
        private Product _product = new();

        public IProductBuilder WithId(int id)
        {
            _product.Id = id;
            return this;
        }

        public IProductBuilder WithName(string name)
        {
            _product.Name = name;
            return this;
        }

        public IProductBuilder WithDescription(string description)
        {
            _product.Description = description;
            return this;
        }

        public IProductBuilder WithPrice(decimal price)
        {
            _product.Price = price;
            return this;
        }

        public IProductBuilder WithImageUrl(string imageUrl)
        {
            _product.ImageUrl = imageUrl;
            return this;
        }

        public IProductBuilder WithCategory(string category)
        {
            _product.Category = category;
            return this;
        }

        public IProductBuilder WithStockQuantity(int quantity)
        {
            _product.StockQuantity = quantity;
            return this;
        }

        public IProductBuilder AsFavorite(bool isFavorite = true)
        {
            _product.IsFavorite = isFavorite;
            return this;
        }

        public IProductBuilder WithCreatedDate(DateTime createdDate)
        {
            _product.CreatedDate = createdDate;
            return this;
        }

        public Product Build()
        {
            if (string.IsNullOrWhiteSpace(_product.Name))
                throw new InvalidOperationException("Product name is required");

            if (_product.Price < 0)
                throw new InvalidOperationException("Product price cannot be negative");

            if (_product.CreatedDate == default)
                _product.CreatedDate = DateTime.Now;

            return _product;
        }
    }
}
