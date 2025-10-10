using System.Text;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace NatOnNat.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            System.Console.OutputEncoding = Encoding.UTF8;

            using var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var productRepository = services.GetRequiredService<IProductRepository>();

                await dbContext.Database.MigrateAsync();

                PrintHeader();

                await ShowIdentityUsers(userManager);
                await ShowAllProducts(productRepository);

                PrintFooter();
            }
            catch (Exception ex)
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine($"Ett fel uppstod: {ex.Message}");
                System.Console.ResetColor();
            }

            System.Console.WriteLine("\nTryck på valfri tangent för att avsluta...");
            System.Console.ReadKey();
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                        ?? "Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

                    services.AddDbContext<ApplicationDbContext>(options =>
                        options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure")));

                    // Identity för Console – använd AddIdentityCore (ingen webbstack krävs)
                    services
                        .AddIdentityCore<IdentityUser>(o =>
                        {
                            o.User.RequireUniqueEmail = true;
                            o.Password.RequireDigit = true;
                            o.Password.RequiredLength = 6;
                            o.Password.RequireNonAlphanumeric = true;
                            o.Password.RequireUppercase = true;
                            o.Password.RequireLowercase = true;
                        })
                        .AddRoles<IdentityRole>()
                        .AddEntityFrameworkStores<ApplicationDbContext>(); // OBS: ingen .AddDefaultTokenProviders() här

                    // Repo
                    services.AddScoped<IProductRepository, ProductRepository>();
                });

        static void PrintHeader()
        {
            System.Console.Clear();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
            System.Console.WriteLine("║                    NätOnNät Admin Console                        ║");
            System.Console.WriteLine("║                    System Administration Tool                    ║");
            System.Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            System.Console.ResetColor();
            System.Console.WriteLine();
        }

        static void PrintFooter()
        {
            System.Console.WriteLine();
            System.Console.ForegroundColor = ConsoleColor.Cyan;
            System.Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
            System.Console.ResetColor();
        }

        static async Task ShowIdentityUsers(UserManager<IdentityUser> userManager)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("📋 REGISTRERADE ANVÄNDARE");
            System.Console.WriteLine("─────────────────────────────────────────────────");
            System.Console.ResetColor();

            var users = await userManager.Users.ToListAsync();

            if (users.Any())
            {
                System.Console.WriteLine($"{"Användarnamn",-35} {"Email",-35} {"Bekräftad",-12}");
                System.Console.WriteLine(new string('─', 82));

                foreach (var user in users)
                {
                    System.Console.ForegroundColor = ConsoleColor.White;
                    System.Console.Write($"{user.UserName,-35} ");
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    System.Console.Write($"{user.Email,-35} ");
                    System.Console.ForegroundColor = user.EmailConfirmed ? ConsoleColor.Green : ConsoleColor.Red;
                    System.Console.WriteLine($"{(user.EmailConfirmed ? "✓ Ja" : "✗ Nej"),-12}");
                    System.Console.ResetColor();
                }

                System.Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine($"✓ Totalt antal användare: {users.Count}");
                System.Console.ResetColor();
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("✗ Inga användare hittades i databasen.");
                System.Console.ResetColor();
            }

            System.Console.WriteLine();
        }

        static async Task ShowAllProducts(IProductRepository productRepository)
        {
            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine("📦 PRODUKTKATALOG");
            System.Console.WriteLine("─────────────────────────────────────────────────");
            System.Console.ResetColor();

            var products = await productRepository.GetAllAsync();

            if (products.Any())
            {
                System.Console.ForegroundColor = ConsoleColor.Cyan;
                System.Console.WriteLine($"{"ID",-4} {"Namn",-25} {"Kategori",-15} {"Pris",-12} {"Lager",-8} {"Favorit",-8} {"Skapad",-12}");
                System.Console.WriteLine(new string('─', 84));
                System.Console.ResetColor();

                foreach (var product in products)
                {
                    if (product.StockQuantity == 0) System.Console.ForegroundColor = ConsoleColor.Red;
                    else if (product.StockQuantity < 10) System.Console.ForegroundColor = ConsoleColor.Yellow;
                    else System.Console.ForegroundColor = ConsoleColor.White;

                    System.Console.Write($"{product.Id,-4} ");
                    System.Console.Write($"{TruncateString(product.Name, 25),-25} ");
                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    System.Console.Write($"{product.Category,-15} ");
                    System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.Write($"{product.Price,10:N0} kr ");

                    if (product.StockQuantity == 0) System.Console.ForegroundColor = ConsoleColor.Red;
                    else if (product.StockQuantity < 10) System.Console.ForegroundColor = ConsoleColor.Yellow;
                    else System.Console.ForegroundColor = ConsoleColor.Green;
                    System.Console.Write($"{product.StockQuantity,8} ");

                    System.Console.ForegroundColor = product.IsFavorite ? ConsoleColor.Yellow : ConsoleColor.Gray;
                    System.Console.Write($"{(product.IsFavorite ? "⭐ Ja" : "   Nej"),8} ");

                    System.Console.ForegroundColor = ConsoleColor.Gray;
                    System.Console.WriteLine($"{product.CreatedDate:yyyy-MM-dd}");
                    System.Console.ResetColor();
                }

                System.Console.WriteLine();
                System.Console.ForegroundColor = ConsoleColor.Green;
                System.Console.WriteLine("📊 SAMMANFATTNING:");
                System.Console.WriteLine($"  • Totalt antal produkter: {products.Count()}");
                System.Console.WriteLine($"  • Favoriter: {products.Count(p => p.IsFavorite)}");
                System.Console.WriteLine($"  • Produkter i lager: {products.Count(p => p.StockQuantity > 0)}");
                System.Console.WriteLine($"  • Produkter slut i lager: {products.Count(p => p.StockQuantity == 0)}");
                System.Console.WriteLine($"  • Total lagervärde: {products.Sum(p => p.Price * p.StockQuantity):N0} kr");
                System.Console.ResetColor();
            }
            else
            {
                System.Console.ForegroundColor = ConsoleColor.Red;
                System.Console.WriteLine("✗ Inga produkter hittades i databasen.");
                System.Console.ResetColor();
            }
        }

        static string TruncateString(string value, int maxLength) =>
            string.IsNullOrEmpty(value) ? value : (value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...");
    }
}
