using ErpConnector.Models;

namespace ErpConnector.Repository.IRepository
{
    public interface INopRepository
    {
        Task<int?> GetProductIdByExternalId(string apiId);
        Task InsertProduct(Product product);
        Task UpdateProduct(Product product, int id);
    }
}