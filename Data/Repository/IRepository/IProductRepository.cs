using ErpConnector.DTOs;

namespace ErpConnector.Repository.IRepository
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductFromApiDto>>  GetProducts();
    }
}
