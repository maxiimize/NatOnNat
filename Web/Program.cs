using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Application.Interfaces;
using Application.Services;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddTransient<SimpleRetryHandler>();

            builder.Services.AddHttpClient<IProductService, ProductService>(c =>
            {
                c.BaseAddress = new Uri(builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7257/");
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            }).AddHttpMessageHandler<SimpleRetryHandler>();

            builder.Services.AddHttpClient("Warmup");

            builder.Services.AddHttpClient<IWeatherService, WeatherService>(c =>
            {
                c.DefaultRequestHeaders.Add("Accept", "application/json");
                c.DefaultRequestHeaders.Add("User-Agent", "NatOnNat-Weather-Client");
            });

            builder.Services.AddHostedService<ApiWarmupHostedService>();

            builder.Services.AddResponseCaching();
            builder.Services.AddMemoryCache();
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseResponseCaching();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }

    public sealed class SimpleRetryHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            for (var i = 0; ; i++)
            {
                try
                {
                    var response = await base.SendAsync(request, cancellationToken);
                    if ((int)response.StatusCode >= 500 && i < 5)
                    {
                        await Task.Delay(200 << i, cancellationToken);
                        continue;
                    }
                    return response;
                }
                catch (HttpRequestException) when (i < 5)
                {
                    await Task.Delay(200 << i, cancellationToken);
                }
            }
        }
    }

    public sealed class ApiWarmupHostedService : IHostedService
    {
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;

        public ApiWarmupHostedService(IHttpClientFactory factory, IConfiguration config)
        {
            _factory = factory;
            _config = config;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var baseUrl = _config["ApiSettings:BaseUrl"] ?? "https://localhost:7257/";
            var client = _factory.CreateClient("Warmup");
            client.BaseAddress = new Uri(baseUrl);
            for (var i = 0; i < 20 && !cancellationToken.IsCancellationRequested; i++)
            {
                try
                {
                    var res = await client.GetAsync("healthz", cancellationToken);
                    if (res.IsSuccessStatusCode) break;
                }
                catch { }
                await Task.Delay(250, cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
