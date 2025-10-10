using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Infrastructure.Data;
using Domain.Interfaces;
using Infrastructure.Repositories;

namespace Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("DefaultConnection")
                 ?? "Server=localhost;Database=NatOnNatDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True";


        services.AddDbContext<ApplicationDbContext>(o =>
            o.UseSqlServer(cs, b => b.MigrationsAssembly("Infrastructure")));

        services.AddScoped<IProductRepository, ProductRepository>();
        return services;
    }
}
