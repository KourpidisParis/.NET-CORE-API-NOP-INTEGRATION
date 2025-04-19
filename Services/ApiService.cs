using ErpConnector.DTOs;
using ErpConnector.Repository.IRepository;
using ErpConnector.Services.IServices;

namespace ErpConnector.Services
{
    public class ApiService : IApiService
    {
        private readonly IApiRepository _apiRepository;

        public ApiService(IApiRepository apiRepository)
        {
            _apiRepository = apiRepository;
        }
        public async Task<IEnumerable<ProductFromApiDto>> GetProducts()
        {
            return await _apiRepository.GetProducts();
        }
        public async Task<IEnumerable<CategoryFromApiDto>> GetCategories()
        {
            return await _apiRepository.GetCategories();
        }
    }
}
