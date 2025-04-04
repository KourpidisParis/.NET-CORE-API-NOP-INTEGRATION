using ErpConnector.Data;
using ErpConnector.Model;
using Microsoft.Extensions.Configuration;

namespace ErpConnector
{
    class Program
    {
        static void Main(string[] args)
        {
            // Load configuration
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var dbContext = new DataContextDapper(config);
            Console.WriteLine("Dapper context initialized!");

            // Fetch products
            IEnumerable<Product> products = dbContext.GetProducts();
            
            // Display products
            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Name}, SKU: {product.Sku}, Price: {product.Price}");
            }
        }
    }
}