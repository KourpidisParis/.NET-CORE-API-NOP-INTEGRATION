using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ErpConnector.Data;
using ErpConnector.Controllers;
using ErpConnector.Services;
using ErpConnector.Repository;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;
using ErpConnector.Mapping;

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

            //Database
            services.AddSingleton<IConfiguration>(configuration);
            services.AddTransient<DataContextDapper>();
            services.AddTransient<IDbInitializer, DbInitializer>();

            //Api
            services.AddHttpClient<IApiRepository, ApiRepository>(); 
            services.AddTransient<IApiService, ApiService>();

            //Nop
            services.AddTransient<INopRepository, NopRepository>(); 
            services.AddTransient<INopService, NopService>();

            //Mapping
            services.AddAutoMapper(typeof(MappingProfile));

            //Controllers
            services.AddTransient<ProductController>();
            services.AddTransient<CategoryController>();

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

            // Initialize database
            var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeProductTableAsync();

            // Check args
            if (args.Length == 0)
            {
                Console.WriteLine("Please provide a command: 'products' or 'categories'");
                return;
            }

            string command = args[0].ToLower();

            switch (command)
            {
                case "products":
                    var productController = serviceProvider.GetRequiredService<ProductController>();
                    await productController.SyncProducts();
                    break;

                case "categories":
                    var categoryController = serviceProvider.GetRequiredService<CategoryController>();
                    await categoryController.SyncCategories();
                    break;

                default:
                    Console.WriteLine("Invalid command. Use 'products' or 'categories'.");
                    break;
            }
        }
    }
}
