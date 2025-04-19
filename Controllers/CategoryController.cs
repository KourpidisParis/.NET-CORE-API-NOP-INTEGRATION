using ErpConnector.Services.IServices;

namespace ErpConnector.Controllers
{
    public class CategoryController
    {
        
        private readonly IApiService _apiService;
        public CategoryController(IApiService apiService)
        {
            _apiService = apiService;
        }
        public async Task SyncCategories()
        {
            var categories = await _apiService.GetCategories();

            foreach (var category in categories)
            {
                Console.WriteLine($"Category: {category.Name}, Slug: {category.Slug}");
            }
        }
    }
}