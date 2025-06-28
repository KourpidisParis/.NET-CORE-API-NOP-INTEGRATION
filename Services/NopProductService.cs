using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace ErpConnector.Services
{
    public class NopProductService : INopProductService
    {
        private readonly ILogger<NopProductService> _logger;
        private readonly INopProductRepository _nopProductRepository;
        private readonly INopLocalizedPropertyService _nopLocalizedPropertyService;
        private readonly IProductMapper _productMapper;
        
        public NopProductService(ILogger<NopProductService> logger, INopProductRepository nopRepository, INopLocalizedPropertyService nopLocalizedPropertyService, IProductMapper productMapper)
        {
            _logger = logger;
            _nopProductRepository = nopRepository;
            _nopLocalizedPropertyService = nopLocalizedPropertyService;
            _productMapper = productMapper;
        }

        public async Task SyncProducts(IEnumerable<ProductFromApiDto> products)
        {
            var totalProducts = products.Count();
            var processedCount = 0;
            var errorCount = 0;
            var skippedCount = 0;

            _logger.LogInformation("Starting sync of {TotalCount} products", totalProducts);

            foreach (var productDto in products)
            {
                try
                {
                    var productModel = _productMapper.MapToProduct(productDto);

                    if (productModel.ApiId == null)
                    {
                        _logger.LogWarning("Skipping product {Name} - null ApiId", productModel.Name);
                        skippedCount++;
                        continue;
                    }

                    var productId = await _nopProductRepository.GetProductIdByExternalId(productModel.ApiId.Value.ToString());

                    if (productId.HasValue)
                    {
                        await _nopProductRepository.UpdateProduct(productModel, productId.Value);
                        _logger.LogDebug("Updated product: {Name} (ID: {Id})", productModel.Name, productId.Value);
                    }
                    else
                    {
                        productId = await _nopProductRepository.InsertProduct(productModel);
                        _logger.LogDebug("Inserted product: {Name} (ID: {Id})", productModel.Name, productId);
                    }

                    // Category mapping with error handling
                    try
                    {
                        await MapProductToCategory(productModel, productId);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to map product {ProductName} to category", productModel.Name);
                        // Continue processing even if category mapping fails
                    }

                    // Localized property with error handling
                    try
                    {
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
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Failed to save localized property for product {ProductName}", productModel.Name);
                        // Continue processing even if localization fails
                    }
                    
                    processedCount++;
                    
                    // Progress indicator for console
                    if (processedCount % 10 == 0)
                    {
                        Console.WriteLine($"📈 Processed {processedCount}/{totalProducts} products...");
                    }
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, "Database error processing product {ProductName}", productDto.Title ?? "Unknown");
                    errorCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing product {ProductName}", productDto.Title ?? "Unknown");
                    errorCount++;
                }
            }

            _logger.LogInformation("Product sync completed. Processed: {Processed}, Errors: {Errors}, Skipped: {Skipped}", 
                processedCount, errorCount, skippedCount);
                
            Console.WriteLine($"📊 Summary: {processedCount} processed, {errorCount} errors, {skippedCount} skipped");

            if (errorCount > 0)
            {
                throw new InvalidOperationException($"Product sync completed with {errorCount} errors");
            }
        }

        public async Task MapProductToCategory(Product productModel, int? productId)
        {
            try
            {
                if (productId <= 0 || productModel == null)
                {
                    _logger.LogWarning("Invalid product or product ID. Cannot map category");
                    return;
                }

                var categoryId = await _nopProductRepository.GetCategoryIdByApiId(productModel.Category);

                if (!categoryId.HasValue)
                {
                    _logger.LogWarning("Category not found for {Category}", productModel.Category);
                    return;
                }

                bool mappingExists = await _nopProductRepository.GetProductCategoryMapping(productId.Value, categoryId.Value);
                
                if (mappingExists)
                {
                    _logger.LogDebug("Mapping already exists for ProductId: {ProductId}, CategoryId: {CategoryId}", productId, categoryId);
                }
                else
                {
                    await _nopProductRepository.InsertProductCategoryMapping(productId.Value, categoryId.Value);
                    _logger.LogDebug("Mapped ProductId {ProductId} to CategoryId {CategoryId}", productId, categoryId.Value);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Database error mapping product {ProductId} to category", productId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error mapping product {ProductId} to category", productId);
                throw;
            }
        }
    }
}
