using ErpConnector.Data;
using ErpConnector.DTOs;
using ErpConnector.Services;

namespace ErpConnector.Controllers
{
    public class ProductController
    {
        private readonly DataContextDapper _dbContext;
        private readonly IProductService _productService;

        public ProductController(IProductService productService, DataContextDapper dbContext)
        {
            _productService = productService;
            _dbContext = dbContext;
        }

        public async Task SyncProducts()
        {
            Console.WriteLine("Dapper context initialized!");

            IEnumerable<ProductFromApiDto> products = await _productService.GetProductsFromApi();

            foreach (var product in products)
            {
                Console.WriteLine($"ID: {product.Id}, Name: {product.Title}, Description: {product.Description}");
            }
        }
    }
}
