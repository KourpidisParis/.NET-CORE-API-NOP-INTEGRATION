using ErpConnector.DTOs;

namespace ErpConnector.Services.IServices
{
    public interface INopCategoryService
    {
        Task SyncCategories(IEnumerable<CategoryFromApiDto> categories);
    }
}
