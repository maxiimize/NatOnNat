using Domain.Entities;
using Domain.Builders;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SeedData(modelBuilder);
        }

        static void SeedData(ModelBuilder builder)
        {
            var products = new[]
            {
                new ProductBuilder().WithId(1).WithName("MacBook Pro 16\"").WithDescription("Kraftfull laptop med M3 Max-chip, 36GB RAM och 1TB SSD").WithPrice(34990m).WithCategory("Laptops").WithImageUrl("https://picsum.photos/640/480?random=1").WithStockQuantity(15).AsFavorite(true).WithCreatedDate(new DateTime(2025, 9, 10)).Build(),
                new ProductBuilder().WithId(2).WithName("iPhone 15 Pro Max").WithDescription("Senaste iPhone med titaniumdesign, A17 Pro-chip och Action Button").WithPrice(16990m).WithCategory("Smartphones").WithImageUrl("https://picsum.photos/640/480?random=2").WithStockQuantity(25).AsFavorite(true).WithCreatedDate(new DateTime(2025, 10, 5)).Build(),
                new ProductBuilder().WithId(3).WithName("Sony WH-1000XM5").WithDescription("Premium brusreducerande hörlurar med branschledande ljudkvalitet").WithPrice(3490m).WithCategory("Headphones").WithImageUrl("https://picsum.photos/640/480?random=3").WithStockQuantity(40).AsFavorite(true).WithCreatedDate(new DateTime(2025, 9, 25)).Build(),
                new ProductBuilder().WithId(4).WithName("Samsung Odyssey G9").WithDescription("49\" curved gaming-monitor med 240Hz och 1ms responstid").WithPrice(12990m).WithCategory("Monitors").WithImageUrl("https://picsum.photos/640/480?random=4").WithStockQuantity(8).AsFavorite(false).WithCreatedDate(new DateTime(2025, 10, 8)).Build(),
                new ProductBuilder().WithId(5).WithName("Keychron K8 Pro").WithDescription("Trådlöst mekaniskt tangentbord med hot-swappable switchar").WithPrice(1690m).WithCategory("Keyboards").WithImageUrl("https://picsum.photos/640/480?random=5").WithStockQuantity(30).AsFavorite(false).WithCreatedDate(new DateTime(2025, 10, 1)).Build(),
                new ProductBuilder().WithId(6).WithName("NVIDIA RTX 4090").WithDescription("Flaggskepps-grafikkort för 4K-gaming och AI-arbetsbelastningar").WithPrice(22990m).WithCategory("Graphics Cards").WithImageUrl("https://picsum.photos/640/480?random=6").WithStockQuantity(5).AsFavorite(true).WithCreatedDate(new DateTime(2025, 10, 9)).Build(),
                new ProductBuilder().WithId(7).WithName("Samsung 990 PRO 2TB").WithDescription("NVMe SSD med läshastigheter upp till 7450 MB/s").WithPrice(2490m).WithCategory("Storage").WithImageUrl("https://picsum.photos/640/480?random=7").WithStockQuantity(50).AsFavorite(false).WithCreatedDate(new DateTime(2025, 9, 20)).Build(),
                new ProductBuilder().WithId(8).WithName("ASUS ROG Strix").WithDescription("Gaming-laptop med RTX 4070, Intel Core i9 och 32GB RAM").WithPrice(24990m).WithCategory("Laptops").WithImageUrl("https://picsum.photos/640/480?random=8").WithStockQuantity(12).AsFavorite(false).WithCreatedDate(new DateTime(2025, 10, 7)).Build(),
                new ProductBuilder().WithId(9).WithName("Ubiquiti Dream Machine").WithDescription("All-in-one nätverkslösning för hem och små kontor").WithPrice(4290m).WithCategory("Networking").WithImageUrl("https://picsum.photos/640/480?random=9").WithStockQuantity(20).AsFavorite(false).WithCreatedDate(new DateTime(2025, 9, 15)).Build(),
                new ProductBuilder().WithId(10).WithName("iPad Pro 12.9\"").WithDescription("Professionell tablet med M2-chip och Liquid Retina XDR-skärm").WithPrice(14990m).WithCategory("Tablets").WithImageUrl("https://picsum.photos/640/480?random=10").WithStockQuantity(18).AsFavorite(true).WithCreatedDate(new DateTime(2025, 10, 3)).Build(),
                new ProductBuilder().WithId(11).WithName("LG C3 OLED 65\"").WithDescription("4K OLED TV perfekt för gaming med HDMI 2.1 och 120Hz").WithPrice(18990m).WithCategory("Monitors").WithImageUrl("https://picsum.photos/640/480?random=11").WithStockQuantity(7).AsFavorite(false).WithCreatedDate(new DateTime(2025, 10, 10, 8, 0, 0)).Build()
            };

            builder.Entity<Product>().HasData(products);
        }
    }
}
