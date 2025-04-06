using ErpConnector.Services.IServices;

namespace ErpConnector.Controllers
{
    public class ProductController
    {
        private readonly IApiService _apiService;
        private readonly INopService _nopService;

        public ProductController(IApiService apiService, INopService syncService)
        {
            _apiService = apiService;
            _nopService = syncService;
        }

        public async Task SyncProducts()
        {
            var products = await _apiService.GetProducts();
            await _nopService.SyncProducts(products);
        }
    }
}
