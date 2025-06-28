using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ErpConnector.Data;
using ErpConnector.Controllers;
using ErpConnector.Repository;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;
using ErpConnector.Services;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Mappers;
using ErpConnector.Models;
using ErpConnector.Data;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace ErpConnector
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ILogger<Program>? logger = null;
            
            try
            {
                // Setup configuration
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                // Setup DI
                var services = new ServiceCollection();

                // Add simple logging
                services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.SetMinimumLevel(LogLevel.Information);
                });

                //Database
                services.AddSingleton<IConfiguration>(configuration);
                services.AddScoped<DataContextDapper>();
                services.AddScoped<IDbInitializer, DbInitializer>();

                //Api
                services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
                services.AddHttpClient<IApiRepository, ApiRepository>(); 
                services.AddTransient<IApiService, ApiService>();

                //Nop - Product
                services.AddTransient<INopProductRepository, NopProductRepository>(); 
                services.AddTransient<INopProductService, NopProductService>();

                //Nop - Category
                services.AddTransient<INopCategoryRepository, NopCategoryRepository>(); 
                services.AddTransient<INopCategoryService, NopCategoryService>();

                //Nop - NopLocalizedProperty
                services.AddTransient<INopLocalizedPropertyRepository, NopLocalizedPropertyRepository>(); 
                services.AddTransient<INopLocalizedPropertyService, NopLocalizedPropertyService>(); 

                //Mappers
                services.AddTransient<IProductMapper, ProductMapper>();
                services.AddTransient<ICategoryMapper, CategoryMapper>();

                //Controllers
                services.AddTransient<ProductController>();
                services.AddTransient<CategoryController>();
                services.AddTransient<TestController>();

                // Build service provider
                var serviceProvider = services.BuildServiceProvider();
                logger = serviceProvider.GetRequiredService<ILogger<Program>>();
                
                logger.LogInformation("Application starting...");

                // Initialize database with error handling
                try
                {
                    var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
                    await dbInitializer.InitializeProductTableAsync();
                    await dbInitializer.InitializeCategoryTableAsync();
                    logger.LogInformation("Database initialized successfully");
                }
                catch (SqlException ex)
                {
                    logger.LogError(ex, "Database initialization failed");
                    Console.WriteLine("❌ ERROR: Cannot connect to database. Please check connection string.");
                    return;
                }

                // Command validation
                if (args.Length == 0)
                {
                    logger.LogWarning("No command provided");
                    Console.WriteLine("Please provide a command: 'products' or 'categories'");
                    return;
                }

                string command = args[0].ToLower();
                logger.LogInformation("Executing command: {Command}", command);

                bool success = false;
                switch (command)
                {
                    case "products":
                        var productController = serviceProvider.GetRequiredService<ProductController>();
                        success = await productController.SyncProducts();
                        break;
                    case "categories":
                        var categoryController = serviceProvider.GetRequiredService<CategoryController>();
                        success = await categoryController.SyncCategories();
                        break;
                    case "test":
                        var testController = serviceProvider.GetRequiredService<TestController>();
                        success = await testController.Main();
                        break;    
                    default:
                        logger.LogWarning("Invalid command provided: {Command}", command);
                        Console.WriteLine("Invalid command. Use 'products' or 'categories'.");
                        return;
                }

                if (success)
                {
                    logger.LogInformation("Application completed successfully");
                    Console.WriteLine("✅ Operation completed successfully");
                }
                else
                {
                    logger.LogError("Application completed with errors");
                    Console.WriteLine("❌ Operation completed with errors");
                    Environment.Exit(1);
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine($"❌ Configuration file not found: {ex.Message}");
                Environment.Exit(1);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"❌ Invalid configuration file: {ex.Message}");
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                logger?.LogCritical(ex, "Unhandled exception in application");
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                Environment.Exit(1);
            }
        }
    }
}
