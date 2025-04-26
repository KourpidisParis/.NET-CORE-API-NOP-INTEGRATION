using AutoMapper;
using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Processors.IProcessor;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class NopProductService : INopProductService
    {
        private readonly INopProductRepository _nopRepository;
        private readonly IMapper _mapper;
        private readonly IProductProcessor _productProcessor;
        public NopProductService(INopProductRepository nopRepository,IMapper mapper,IProductProcessor productProcessor)
        {
            _nopRepository = nopRepository;
            _mapper = mapper;
            _productProcessor = productProcessor;
        }

        public async Task SyncProducts(IEnumerable<ProductFromApiDto> products)
        {
            foreach (var productDto in products)
            {
                // var productModel = _mapper.Map<Product>(productDto);
                var productModel = _productProcessor.ApplyDefaultProductValues(_mapper.Map<Product>(productDto));

                if (productModel.ApiId == null)
                {
                    Console.WriteLine($"Skipped: {productModel.Name} (null ApiId)");
                    continue;
                }

                var productId = await _nopRepository.GetProductIdByExternalId(productModel.ApiId.Value.ToString());

                if (productId.HasValue)
                {
                    await _nopRepository.UpdateProduct(productModel, productId.Value);
                    Console.WriteLine($"Updated: {productModel.Name}");
                }
                else
                {
                    productId = await _nopRepository.InsertProduct(productModel);
                    Console.WriteLine($"Inserted: {productModel.Name}");
                }

                //After inser/update product
                var categoryId = await _nopRepository.GetCategoryIdByApiId(productModel.Category);
                if (categoryId.HasValue)
                {
                    await _nopRepository.InsertProductCategoryMapping(productId.Value, categoryId.Value);
                }
            }
        }
    }
}