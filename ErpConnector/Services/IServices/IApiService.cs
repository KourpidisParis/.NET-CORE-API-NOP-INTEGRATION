using ErpConnector.DTOs;

namespace ErpConnector.Services.IServices
{
    public interface IApiService
    {
        Task<IEnumerable<ProductFromApiDto>> GetProducts();
        Task<IEnumerable<CategoryFromApiDto>> GetCategories();
    }
}
