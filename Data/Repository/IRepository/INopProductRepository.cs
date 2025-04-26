using ErpConnector.Models;

namespace ErpConnector.Repository.IRepository
{
    public interface INopProductRepository
    {
        Task<int?> GetProductIdByExternalId(string apiId);
        Task<int> InsertProduct(Product product);
        Task UpdateProduct(Product product, int id);
        Task<int?> GetCategoryIdByApiId(string apiId);
        Task InsertProductCategoryMapping(int productId, int categoryId);
    }
}