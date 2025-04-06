using ErpConnector.DTOs;

namespace ErpConnector.Repository.IRepository
{
    public interface INopRepository
    {
        Task<int?> GetProductIdByExternalId(string externalId);
        Task InsertProduct(ProductFromApiDto product);
        Task UpdateProduct(ProductFromApiDto product, int id);
    }
}