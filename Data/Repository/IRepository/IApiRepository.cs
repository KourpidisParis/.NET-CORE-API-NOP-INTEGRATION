using ErpConnector.DTOs;

namespace ErpConnector.Repository.IRepository
{
    public interface IApiRepository
    {
        Task<IEnumerable<ProductFromApiDto>> GetProducts();
        Task<IEnumerable<CategoryFromApiDto>> GetCategories();
    }
}
