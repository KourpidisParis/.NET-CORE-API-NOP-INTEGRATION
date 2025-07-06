using ErpConnector.Models;

namespace ErpConnector.Repository.IRepository
{
    public interface INopCategoryRepository
    {
        Task<int?> GetCategoryIdByExternalId(string apiId);
        Task InsertCategory(Category category);
        Task UpdateCategory(Category category, int id);
    }
}
