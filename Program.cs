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

            //Service Provider
            var serviceProvider = services.BuildServiceProvider();

            //Initialize data
            var dbInitializer = serviceProvider.GetRequiredService<IDbInitializer>();
            await dbInitializer.InitializeProductTableAsync();

            //Run the main logic
            var controller = serviceProvider.GetRequiredService<ProductController>();
            await controller.SyncProducts();
        }
    }
}
