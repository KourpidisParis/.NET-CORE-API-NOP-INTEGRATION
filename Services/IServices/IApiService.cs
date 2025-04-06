using ErpConnector.DTOs;

namespace ErpConnector.Services.IServices
{
    public interface IApiService
    {
        Task<IEnumerable<ProductFromApiDto>> GetProducts();
    }
}
