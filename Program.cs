using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using System.Net.Http;
using ErpConnector.Data;
using ErpConnector.Models;
using ErpConnector.Controllers;
using ErpConnector.Services;
using ErpConnector.Repository.IRepository;
using ErpConnector.Repository;

namespace ErpConnector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // Setup configuration
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Setup DI
            var services = new ServiceCollection();

            services.AddSingleton<IConfiguration>(configuration);

            services.AddHttpClient<IProductRepository, ProductRepository>(); 

            services.AddTransient<IProductService, ProductService>();

            services.AddTransient<DataContextDapper>();

            services.AddTransient<ProductController>();

            // Build DI provider
            var serviceProvider = services.BuildServiceProvider();

            // Run the main logic
            var controller = serviceProvider.GetRequiredService<ProductController>();
            await controller.SyncProducts();
        }
    }
}
