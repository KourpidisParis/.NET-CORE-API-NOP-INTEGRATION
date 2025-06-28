using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace ErpConnector.Services
{
    public class NopCategoryService : INopCategoryService
    {
        private readonly ILogger<NopCategoryService> _logger;
        private readonly INopCategoryRepository _nopRepository;
        private readonly ICategoryMapper _categoryMapper;
        
        public NopCategoryService(ILogger<NopCategoryService> logger, INopCategoryRepository nopRepository, ICategoryMapper categoryMapper)
        {
            _logger = logger;
            _nopRepository = nopRepository;
            _categoryMapper = categoryMapper;
        }

        public async Task SyncCategories(IEnumerable<CategoryFromApiDto> categories)
        {
            var totalCategories = categories.Count();
            var processedCount = 0;
            var errorCount = 0;
            var skippedCount = 0;

            _logger.LogInformation("Starting sync of {TotalCount} categories", totalCategories);

            foreach (var categoryDto in categories)
            {
                try
                {
                    var categoryModel = _categoryMapper.MapToCategory(categoryDto);

                    if (string.IsNullOrEmpty(categoryModel.ApiId))
                    {
                        _logger.LogWarning("Skipping category {Name} - null or empty ApiId", categoryModel.Name);
                        skippedCount++;
                        continue;
                    }

                    var existingId = await _nopRepository.GetCategoryIdByExternalId(categoryModel.ApiId);

                    if (existingId.HasValue)
                    {
                        await _nopRepository.UpdateCategory(categoryModel, existingId.Value);
                        _logger.LogDebug("Updated category: {Name} (ID: {Id})", categoryModel.Name, existingId.Value);
                    }
                    else
                    {
                        await _nopRepository.InsertCategory(categoryModel);
                        _logger.LogDebug("Inserted category: {Name}", categoryModel.Name);
                    }
                    
                    processedCount++;
                    
                    // Progress indicator for console
                    if (processedCount % 5 == 0)
                    {
                        Console.WriteLine($"📈 Processed {processedCount}/{totalCategories} categories...");
                    }
                }
                catch (SqlException ex)
                {
                    _logger.LogError(ex, "Database error processing category {CategoryName}", categoryDto.Name ?? "Unknown");
                    errorCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Unexpected error processing category {CategoryName}", categoryDto.Name ?? "Unknown");
                    errorCount++;
                }
            }

            _logger.LogInformation("Category sync completed. Processed: {Processed}, Errors: {Errors}, Skipped: {Skipped}", 
                processedCount, errorCount, skippedCount);
                
            Console.WriteLine($"📊 Summary: {processedCount} processed, {errorCount} errors, {skippedCount} skipped");

            if (errorCount > 0)
            {
                throw new InvalidOperationException($"Category sync completed with {errorCount} errors");
            }
        }
    }
}
