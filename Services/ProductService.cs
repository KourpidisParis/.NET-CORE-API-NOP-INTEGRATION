using ErpConnector.DTOs;
using ErpConnector.Repository.IRepository;

namespace ErpConnector.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<ProductFromApiDto>> GetProductsFromApi()
        {
            return await _productRepository.GetProducts();
        }
    }
}
