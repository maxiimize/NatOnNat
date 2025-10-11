using System.Text;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Spectre.Console;

namespace NatOnNat.ConsoleApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            using var host = CreateHostBuilder(args).Build();
            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var dbContext = services.GetRequiredService<ApplicationDbContext>();
                var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
                var productRepository = services.GetRequiredService<IProductRepository>();

                await dbContext.Database.MigrateAsync();

                var adminMenu = new AdminMenu(userManager, productRepository);
                await adminMenu.Run();
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Ett fel uppstod: {Markup.Escape(ex.Message)}[/]");
                AnsiConsole.MarkupLine("[grey]Tryck på valfri tangent för att avsluta...[/]");
                Console.ReadKey();
            }
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    // Stäng av Entity Framework Core loggning för konsolen
                    logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
                    logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
                })
                .ConfigureServices((context, services) =>
                {
                    var connectionString = context.Configuration.GetConnectionString("DefaultConnection")
                        ?? "Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));
                        // Stäng av känslig data-loggning
                        options.EnableSensitiveDataLogging(false);
                        options.EnableDetailedErrors(false);
                    });

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
                        .AddEntityFrameworkStores<ApplicationDbContext>();

                    // Repo
                    services.AddScoped<IProductRepository, ProductRepository>();
                });
    }

    public class AdminMenu
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IProductRepository _productRepository;

        public AdminMenu(UserManager<IdentityUser> userManager, IProductRepository productRepository)
        {
            _userManager = userManager;
            _productRepository = productRepository;
        }

        public async Task Run()
        {
            bool exit = false;
            while (!exit)
            {
                ShowHeader();
                int choice = await PromptChoice();
                Console.Clear();

                switch (choice)
                {
                    case 1:
                        await ShowIdentityUsers();
                        break;
                    case 2:
                        await ShowAllProducts();
                        break;
                    case 3:
                        await SearchProducts();
                        break;
                    case 4:
                        AnsiConsole.MarkupLine("[grey]Avslutar NätOnNät Admin Console...[/]");
                        exit = true;
                        break;
                }

                if (!exit)
                {
                    Console.Clear();
                }
            }
        }

        private void ShowHeader()
        {
            AnsiConsole.Clear();
            AnsiConsole.Write(
                new FigletText("NÄTONNÄT")
                    .Centered()
                    .Color(Color.Cyan1));
            AnsiConsole.Write(
                new Markup("[blue]Admin Console[/]")
                    .Centered());
            AnsiConsole.Write(new Rule());
        }

        private async Task<int> PromptChoice()
        {
            var options = new[]
            {
                "1. Visa registrerade användare",
                "2. Visa produktkatalog",
                "3. Sök produkter",
                "4. Avsluta"
            };

            int maxLen = options.Max(o => o.Length);
            int consoleWidth = Console.WindowWidth;
            int indent = Math.Max((consoleWidth - maxLen) / 2, 0);
            var padding = new string(' ', indent);

            var padded = options.Select(o => padding + o).ToArray();

            AnsiConsole.Write(
                new Markup("[yellow]Välj ett alternativ:[/]")
                    .Centered());
            AnsiConsole.Write(new Rule());

            var selection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .PageSize(padded.Length)
                    .AddChoices(padded)
            );

            var trimmed = selection.TrimStart();
            return await Task.FromResult(int.Parse(trimmed.Split('.')[0]));
        }

        private async Task ShowIdentityUsers()
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .StartAsync("Hämtar användare...", async ctx =>
                {
                    var users = await _userManager.Users.ToListAsync();

                    AnsiConsole.Write(
                        new FigletText("ANVÄNDARE")
                            .Centered()
                            .Color(Color.Yellow));
                    AnsiConsole.Write(new Rule());

                    if (users.Any())
                    {
                        var table = new Table()
                            .Border(TableBorder.Rounded)
                            .AddColumn("[yellow]Användarnamn[/]")
                            .AddColumn("[yellow]Email[/]")
                            .AddColumn("[yellow]Bekräftad[/]")
                            .Centered();

                        foreach (var user in users)
                        {
                            table.AddRow(
                                user.UserName ?? "[grey]N/A[/]",
                                user.Email ?? "[grey]N/A[/]",
                                user.EmailConfirmed ? "[green]✓ Ja[/]" : "[red]✗ Nej[/]"
                            );
                        }

                        AnsiConsole.Write(table);

                        AnsiConsole.WriteLine();
                        var panel = new Panel($"[green]✓ Totalt antal användare: {users.Count}[/]")
                            .Border(BoxBorder.Rounded)
                            .Padding(1, 0);
                        AnsiConsole.Write(panel);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]✗ Inga användare hittades i databasen.[/]");
                    }
                });

            AnsiConsole.MarkupLine("\n[grey]Tryck på valfri tangent för att fortsätta...[/]");
            Console.ReadKey();
        }

        private async Task ShowAllProducts()
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .StartAsync("Hämtar produkter...", async ctx =>
                {
                    var products = await _productRepository.GetAllAsync();

                    AnsiConsole.Write(
                        new FigletText("PRODUKTER")
                            .Centered()
                            .Color(Color.Aqua));
                    AnsiConsole.Write(new Rule());

                    if (products.Any())
                    {
                        var table = new Table()
                            .Border(TableBorder.Rounded)
                            .AddColumn("[cyan]ID[/]")
                            .AddColumn("[cyan]Namn[/]")
                            .AddColumn("[cyan]Kategori[/]")
                            .AddColumn("[cyan]Pris[/]")
                            .AddColumn("[cyan]Lager[/]")
                            .AddColumn("[cyan]Favorit[/]")
                            .AddColumn("[cyan]Skapad[/]")
                            .Centered();

                        foreach (var product in products)
                        {
                            var stockColor = product.StockQuantity == 0 ? "red" :
                                           product.StockQuantity < 10 ? "yellow" : "green";

                            table.AddRow(
                                product.Id.ToString(),
                                Markup.Escape(TruncateString(product.Name, 25)),
                                Markup.Escape(product.Category),
                                $"[green]{product.Price:N0} kr[/]",
                                $"[{stockColor}]{product.StockQuantity}[/]",
                                product.IsFavorite ? "[yellow]⭐ Ja[/]" : "[grey]Nej[/]",
                                $"[grey]{product.CreatedDate:yyyy-MM-dd}[/]"
                            );
                        }

                        AnsiConsole.Write(table);
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[red]✗ Inga produkter hittades i databasen.[/]");
                    }
                });

            AnsiConsole.MarkupLine("\n[grey]Tryck på valfri tangent för att fortsätta...[/]");
            Console.ReadKey();
        }

        private async Task SearchProducts()
        {
            var searchTerm = AnsiConsole.Prompt(
                new TextPrompt<string>("[yellow]Sök efter produkt (namn eller kategori):[/]")
                    .AllowEmpty());

            if (string.IsNullOrWhiteSpace(searchTerm))
                return;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Star)
                .StartAsync($"Söker efter '{searchTerm}'...", async ctx =>
                {
                    var products = await _productRepository.GetAllAsync();
                    var filtered = products.Where(p =>
                        p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                        p.Category.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));

                    if (filtered.Any())
                    {
                        var table = new Table()
                            .Border(TableBorder.Rounded)
                            .Title($"[yellow]Sökresultat för '{searchTerm}'[/]")
                            .AddColumn("ID")
                            .AddColumn("Namn")
                            .AddColumn("Kategori")
                            .AddColumn("Pris")
                            .AddColumn("Lager")
                            .Centered();

                        foreach (var product in filtered)
                        {
                            table.AddRow(
                                product.Id.ToString(),
                                Markup.Escape(product.Name),
                                Markup.Escape(product.Category),
                                $"{product.Price:N0} kr",
                                product.StockQuantity.ToString()
                            );
                        }

                        AnsiConsole.Write(table);
                        AnsiConsole.MarkupLine($"\n[green]Hittade {filtered.Count()} produkt(er)[/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine($"[red]Inga produkter hittades för sökningen '{searchTerm}'[/]");
                    }
                });

            AnsiConsole.MarkupLine("\n[grey]Tryck på valfri tangent för att fortsätta...[/]");
            Console.ReadKey();
        }

        private static string TruncateString(string value, int maxLength) =>
            string.IsNullOrEmpty(value) ? value :
            (value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...");
    }
}