using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Builders
{
    public interface IProductBuilder
    {
        IProductBuilder WithId(int id);
        IProductBuilder WithName(string name);
        IProductBuilder WithDescription(string description);
        IProductBuilder WithPrice(decimal price);
        IProductBuilder WithImageUrl(string imageUrl);
        IProductBuilder WithCategory(string category);
        IProductBuilder WithStockQuantity(int quantity);
        IProductBuilder AsFavorite(bool isFavorite = true);
        IProductBuilder WithCreatedDate(DateTime createdDate);
        Product Build();
    }
}
