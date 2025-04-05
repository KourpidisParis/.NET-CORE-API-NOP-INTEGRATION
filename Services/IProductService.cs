using ErpConnector.DTOs;

namespace ErpConnector.Services
{
    public interface IProductService
    {
        Task<IEnumerable<ProductFromApiDto>>  GetProducts();
    }
}
