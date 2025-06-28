using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace ErpConnector.Controllers
{
    public class ProductController
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IApiService _apiService;
        private readonly INopProductService _nopService;

        public ProductController(ILogger<ProductController> logger, IApiService apiService, INopProductService syncService)
        {
            _logger = logger;
            _apiService = apiService;
            _nopService = syncService;
        }

        public async Task<bool> SyncProducts()
        {
            try
            {
                _logger.LogInformation("Starting product synchronization");
                Console.WriteLine("🔄 Fetching products from API...");
                
                var products = await _apiService.GetProducts();
                
                Console.WriteLine($"📦 Found {products.Count()} products. Syncing to database...");
                await _nopService.SyncProducts(products);
                
                _logger.LogInformation("Product synchronization completed successfully");
                Console.WriteLine("✅ Product synchronization completed");
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during product synchronization");
                Console.WriteLine("❌ Network error: Cannot reach API");
                return false;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Data format error during product sync");
                Console.WriteLine("❌ Data format error: Invalid API response");
                return false;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error during product synchronization");
                Console.WriteLine("❌ Database error: Cannot save products");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during product synchronization");
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                return false;
            }
        }
    }
}
