using ErpConnector.Models;

namespace ErpConnector.Repository.IRepository
{
    public interface INopProductRepository
    {
        //Product Base
        Task<int?> GetProductIdByExternalId(string apiId);
        Task<int> InsertProduct(Product product);
        Task UpdateProduct(Product product, int id);
        
        //Product to Category
        Task<int?> GetCategoryIdByApiId(string? apiId);
        Task InsertProductCategoryMapping(int productId, int categoryId);
        Task<bool> GetProductCategoryMapping(int productId, int categoryId);
    }
}