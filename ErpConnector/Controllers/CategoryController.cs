using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;
using System.Text.Json;

namespace ErpConnector.Controllers
{
    public class CategoryController
    {
        private readonly ILogger<CategoryController> _logger;
        private readonly IApiService _apiService;
        private readonly INopCategoryService _nopCategoryService;
        
        public CategoryController(ILogger<CategoryController> logger, IApiService apiService, INopCategoryService nopCategoryService)
        {
            _logger = logger;
            _apiService = apiService;
            _nopCategoryService = nopCategoryService;
        }
        
        public async Task<bool> SyncCategories()
        {
            try
            {
                _logger.LogInformation("Starting category synchronization");
                Console.WriteLine("🔄 Fetching categories from API...");
                
                var categories = await _apiService.GetCategories();
                
                Console.WriteLine($"📁 Found {categories.Count()} categories. Syncing to database...");
                await _nopCategoryService.SyncCategories(categories);
                
                _logger.LogInformation("Category synchronization completed successfully");
                Console.WriteLine("✅ Category synchronization completed");
                return true;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Network error during category synchronization");
                Console.WriteLine("❌ Network error: Cannot reach API");
                return false;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Data format error during category sync");
                Console.WriteLine("❌ Data format error: Invalid API response");
                return false;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error during category synchronization");
                Console.WriteLine("❌ Database error: Cannot save categories");
                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during category synchronization");
                Console.WriteLine($"❌ Unexpected error: {ex.Message}");
                return false;
            }
        }
    }
}
