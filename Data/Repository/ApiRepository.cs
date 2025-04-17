using System.Text.Json;
using ErpConnector.DTOs;
using ErpConnector.Repository.IRepository;

namespace ErpConnector.Repository
{
    public class ApiRepository : IApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly string _url;
        public ApiRepository(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _url        = "https://dummyjson.com/";
        }
        public async Task<IEnumerable<ProductFromApiDto>> GetProducts()
        {
            var productsUrl = _url + "products";
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

        public async Task<IEnumerable<CategroryFromApiDto>> GetCategories()
        {
            var categoriesUrl = _url + "products/categories";
            var response = await _httpClient.GetAsync(categoriesUrl);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception("Fetch failed.");
            }

            var json = await response.Content.ReadAsStringAsync();

            var products = JsonSerializer.Deserialize<List<CategroryFromApiDto>>(json, new JsonSerializerOptions
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
