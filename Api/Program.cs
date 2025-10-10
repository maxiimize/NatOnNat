using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var cs = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? "Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
;

            builder.Services.AddDbContext<ApplicationDbContext>(o =>
                o.UseSqlServer(cs, b => b.MigrationsAssembly("Infrastructure")));

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

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var db = services.GetRequiredService<ApplicationDbContext>();
                await db.Database.MigrateAsync();
                await IdentitySeeder.SeedAsync(services);
            }

            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
            }

            app.UseHttpsRedirection();
            app.UseCors("AllowMvcApp");
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}
