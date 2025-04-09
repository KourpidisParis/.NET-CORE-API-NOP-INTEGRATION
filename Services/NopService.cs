using AutoMapper;
using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class NopService : INopService
    {
        private readonly INopRepository _nopRepository;
        private readonly IMapper _mapper;
        public NopService(INopRepository nopRepository,IMapper mapper)
        {
            _nopRepository = nopRepository;
            _mapper = mapper;
        }

        public async Task SyncProducts(IEnumerable<ProductFromApiDto> products)
        {
            foreach (var productDto in products)
            {
                var productModel = _mapper.Map<Product>(productDto);

                var existingId = await _nopRepository.GetProductIdByExternalId(productModel.ApiId.ToString());

                productModel.ProductTypeId = 5;

                if (existingId.HasValue)
                {
                    await _nopRepository.UpdateProduct(productModel, existingId.Value);
                    Console.WriteLine($"Updated: {productModel.Name}");
                }
                else
                {
                    await _nopRepository.InsertProduct(productModel);
                    Console.WriteLine($"Inserted: {productModel.Name}");
                }
            }
        }
    }
}