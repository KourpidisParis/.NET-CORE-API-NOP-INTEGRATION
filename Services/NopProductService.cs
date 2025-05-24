using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class NopProductService : INopProductService
    {
        private readonly INopProductRepository _nopProductRepository;
        private readonly INopLocalizedPropertyService _nopLocalizedPropertyService;
        private readonly IProductMapper _productMapper;
        public NopProductService(INopProductRepository nopRepository,INopLocalizedPropertyService nopLocalizedPropertyService,IProductMapper productMapper)
        {
            _nopProductRepository = nopRepository;
            _nopLocalizedPropertyService = nopLocalizedPropertyService;
            _productMapper = productMapper;
        }

        public async Task SyncProducts(IEnumerable<ProductFromApiDto> products)
        {
            foreach (var productDto in products)
            {
                var productModel = _productMapper.MapToProduct(productDto);

                if (productModel.ApiId == null)
                {
                    Console.WriteLine($"Skipped: {productModel.Name} (null ApiId)");
                    continue;
                }

                var productId = await _nopProductRepository.GetProductIdByExternalId(productModel.ApiId.Value.ToString());

                if (productId.HasValue)
                {
                    await _nopProductRepository.UpdateProduct(productModel, productId.Value);
                    Console.WriteLine($"Updated: {productModel.Name}");
                }
                else
                {
                    productId = await _nopProductRepository.InsertProduct(productModel);
                    Console.WriteLine($"Inserted: {productModel.Name}");
                }

                //Connect product with category
                await MapProductToCategory(productModel, productId);

                //Add Name for all languages
                 var localizedPropertyObject = new LocalizedProperty
                {
                    LocaleKeyGroup = "Product",
                    LocaleKey = "Name",
                    LocaleValue = productModel.Name,
                    LanguageId = 2,  
                    EntityId = (int)productId
                };
                
                await _nopLocalizedPropertyService.HandleLocalizedProperty(localizedPropertyObject);
            }
        }

        public async Task MapProductToCategory(Product productModel, int? productId)
        {
            if (productId <= 0 || productModel == null)
            {
                Console.WriteLine("Invalid product or product ID. Cannot map category.");
                return;
            }

            var categoryId = await _nopProductRepository.GetCategoryIdByApiId(productModel.Category);

            if (!categoryId.HasValue)
            {
                Console.WriteLine($"Category not found for {productModel.Category}");
                return;
            }

            bool mappingExists =  _nopProductRepository.GetProductCategoryMapping(productId.Value, categoryId.Value);
            
            if (mappingExists)
            {
                Console.WriteLine($"Mapping already exists for ProductId: {productId}, CategoryId: {categoryId}");
            }
            else
            {
                await _nopProductRepository.InsertProductCategoryMapping(productId.Value, categoryId.Value);
                Console.WriteLine($"Mapped ProductId {productId} to CategoryId {categoryId.Value}");
            }
        }
    }
}