using AutoMapper;
using ErpConnector.DTOs;
using ErpConnector.Helpers;
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
                // var productModel = _mapper.Map<Product>(productDto);
                var productModel = ProductDefaultsHelper.ApplyDefaultProductValues(_mapper.Map<Product>(productDto));

                if (productModel.ApiId == null)
                {
                    Console.WriteLine($"Skipped: {productModel.Name} (null ApiId)");
                    continue;
                }

                var existingId = await _nopRepository.GetProductIdByExternalId(productModel.ApiId.Value.ToString());

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