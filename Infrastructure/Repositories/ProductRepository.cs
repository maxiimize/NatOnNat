using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
{
    
}

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) => _db = db;

        public async Task<IEnumerable<Product>> GetAllAsync() =>
            await _db.Products.AsNoTracking().ToListAsync();

        public async Task<Product?> GetByIdAsync(int id) =>
            await _db.Products.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);

        public async Task<IEnumerable<Product>> GetFavoritesAsync(int count = 3) =>
            await _db.Products.AsNoTracking()
                .Where(p => p.IsFavorite)
                .OrderByDescending(p => p.CreatedDate)
                .Take(count)
                .ToListAsync();

        public async Task<IEnumerable<Product>> GetNewestAsync(int count = 3) =>
            await _db.Products.AsNoTracking()
                .OrderByDescending(p => p.CreatedDate)
                .Take(count)
                .ToListAsync();

        public async Task<Product> AddAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();
            return product;
        }

        public async Task UpdateAsync(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (entity == null) return;
            _db.Products.Remove(entity);
            await _db.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id) =>
            await _db.Products.AnyAsync(p => p.Id == id);
    }
}
