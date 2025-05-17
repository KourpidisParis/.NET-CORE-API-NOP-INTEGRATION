using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Mappers.IMappers;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class NopCategoryService : INopCategoryService
    {
        private readonly INopCategoryRepository _nopRepository;
        private readonly ICategoryMapper _categoryMapper;
        public NopCategoryService(INopCategoryRepository nopRepository, ICategoryMapper categoryMapper)
        {
            _nopRepository = nopRepository;
            _categoryMapper = categoryMapper;
        }

        public async Task SyncCategories(IEnumerable<CategoryFromApiDto> categories)
        {
            foreach (var categoryDto in categories)
            {
                var categoryModel = _categoryMapper.MapToCategory(categoryDto);

                if (string.IsNullOrEmpty(categoryModel.ApiId))
                {
                    Console.WriteLine($"Skipped: {categoryModel.Name} (null ApiId)");
                    continue;
                }

                var existingId = await _nopRepository.GetCategoryIdByExternalId(categoryModel.ApiId);

                if (existingId.HasValue)
                {
                    await _nopRepository.UpdateCategory(categoryModel, existingId.Value);
                    Console.WriteLine($"Updated: {categoryModel.Name}");
                }
                else
                {
                    await _nopRepository.InsertCategory(categoryModel);
                    Console.WriteLine($"Inserted: {categoryModel.Name}");
                }
            }
        }
    }
}
