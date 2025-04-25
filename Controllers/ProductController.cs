using ErpConnector.Services.IServices;

namespace ErpConnector.Controllers
{
    public class ProductController
    {
        private readonly IApiService _apiService;
        private readonly INopProductService _nopService;

        public ProductController(IApiService apiService, INopProductService syncService)
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
