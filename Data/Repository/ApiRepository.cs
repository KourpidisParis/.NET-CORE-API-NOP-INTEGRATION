using System.Text.Json;
using ErpConnector.DTOs;
using ErpConnector.Repository.IRepository;

namespace ErpConnector.Repository
{
    public class ApiRepository : IApiRepository
    {
        private readonly HttpClient _httpClient;
        public ApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<IEnumerable<ProductFromApiDto>> GetProducts()
        {
            var response = await _httpClient.GetAsync("https://fakestoreapi.com/products");

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fetch failed.");
            }

            var json = await response.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<List<ProductFromApiDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (products == null)
            {
                throw new Exception("Failed to deserialize products from ERP.");
            }

            return products;
        }
    }
}
