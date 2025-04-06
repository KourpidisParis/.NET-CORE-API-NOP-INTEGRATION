using ErpConnector.DTOs;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class NopService : INopService
    {
        private readonly INopRepository _nopRepository;

        public NopService(INopRepository nopRepository)
        {
            _nopRepository = nopRepository;
        }

        public async Task SyncProducts(IEnumerable<ProductFromApiDto> products)
        {
            foreach (var product in products)
            {
                var existingId = await _nopRepository.GetProductIdByExternalId(product.Id.ToString());

                if (existingId.HasValue)
                {
                    await _nopRepository.UpdateProduct(product, existingId.Value);
                    Console.WriteLine($"Updated: {product.Title}");
                }
                else
                {
                    await _nopRepository.InsertProduct(product);
                    Console.WriteLine($"Inserted: {product.Title}");
                }
            }
        }
    }
}