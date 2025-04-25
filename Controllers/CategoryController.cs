using ErpConnector.Services.IServices;

namespace ErpConnector.Controllers
{
    public class CategoryController
    {
        private readonly IApiService _apiService;
        private readonly INopCategoryService _nopCategoryService;

        public CategoryController(IApiService apiService, INopCategoryService nopCategoryService)
        {
            _apiService = apiService;
            _nopCategoryService = nopCategoryService;
        }

        public async Task SyncCategories()
        {
            var categories = await _apiService.GetCategories();
            await _nopCategoryService.SyncCategories(categories);
        }
    }
}
