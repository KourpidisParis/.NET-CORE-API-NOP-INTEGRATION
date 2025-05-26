using System.Text.Json;
using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Repository.IRepository;
using Microsoft.Extensions.Options;


namespace ErpConnector.Repository
{
    public class ApiRepository : IApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;

        public ApiRepository(HttpClient httpClient,IOptions<ApiSettings> apiSettings)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
        }
        
        public async Task<IEnumerable<ProductFromApiDto>> GetProducts()
        {
            var productsUrl = _apiSettings.BaseUrl.TrimEnd('/') + "/" + _apiSettings.ProductsEndpoint;

            var response = await _httpClient.GetAsync(productsUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fetch failed.");
            }

            var json = await response.Content.ReadAsStringAsync();

            var productsResponse = JsonSerializer.Deserialize<ProductsResponseDto>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (productsResponse?.Products == null)
            {
                throw new Exception("Failed to deserialize products from ERP.");
            }

            return productsResponse.Products;
        }

        public async Task<IEnumerable<CategoryFromApiDto>> GetCategories()
        {
            var categoriesUrl = _apiSettings.BaseUrl.TrimEnd('/') + "/" + _apiSettings.CategoriesEndpoint;
            
            var response = await _httpClient.GetAsync(categoriesUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fetch failed.");
            }

            var json = await response.Content.ReadAsStringAsync();

            var categories = JsonSerializer.Deserialize<List<CategoryFromApiDto>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (categories == null)
            {
                throw new Exception("Failed to deserialize products from ERP.");
            }

            return categories;
        }
    }
}
