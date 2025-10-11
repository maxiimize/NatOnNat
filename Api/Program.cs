using System.Data.Common;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var cs = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? "Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True;Connect Timeout=30";

            builder.Services.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlServer(cs, b =>
                {
                    b.MigrationsAssembly("Infrastructure");
                    b.CommandTimeout(60);
                    b.EnableRetryOnFailure(3, TimeSpan.FromSeconds(2), null);
                }));

            builder.Services.AddIdentity<IdentityUser, IdentityRole>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequiredLength = 6;
                o.Password.RequireNonAlphanumeric = true;
                o.Password.RequireUppercase = true;
                o.Password.RequireLowercase = true;
                o.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddScoped<IProductRepository, ProductRepository>();

            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMvcApp", p =>
                    p.WithOrigins("https://localhost:7001", "http://localhost:5001")
                     .AllowAnyHeader()
                     .AllowAnyMethod());
            });

            var app = builder.Build();

            await MigrateAndSeedAsync(app);

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.MapGet("/healthz", () => Results.Ok("OK"));

            app.UseHttpsRedirection();
            app.UseCors("AllowMvcApp");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            await app.RunAsync();
        }

        static async Task MigrateAndSeedAsync(IHost app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var db = services.GetRequiredService<ApplicationDbContext>();
            await WarmSqlAsync(db.Database.GetDbConnection());
            var strategy = db.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                var pending = await db.Database.GetPendingMigrationsAsync();
                if (pending.Any())
                {
                    try
                    {
                        await db.Database.MigrateAsync();
                    }
                    catch (SqlException ex) when (ex.Number == 1801)
                    {
                    }
                }
                await IdentitySeeder.SeedAsync(services);
            });
        }

        static async Task WarmSqlAsync(DbConnection connection)
        {
            for (var i = 0; i < 10; i++)
            {
                try
                {
                    await connection.OpenAsync();
                    await connection.CloseAsync();
                    return;
                }
                catch
                {
                    await Task.Delay(300 * (i + 1));
                }
            }
        }
    }
}
