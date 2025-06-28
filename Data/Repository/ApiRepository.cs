using System.Text.Json;
using ErpConnector.DTOs;
using ErpConnector.Models;
using ErpConnector.Repository.IRepository;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace ErpConnector.Repository
{
    public class ApiRepository : IApiRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ApiSettings _apiSettings;
        private readonly ILogger<ApiRepository> _logger;

        public ApiRepository(HttpClient httpClient, IOptions<ApiSettings> apiSettings, ILogger<ApiRepository> logger)
        {
            _httpClient = httpClient;
            _apiSettings = apiSettings.Value;
            _logger = logger;
        }
        
        public async Task<IEnumerable<ProductFromApiDto>> GetProducts()
        {
            try
            {
                var productsUrl = _apiSettings.BaseUrl.TrimEnd('/') + "/" + _apiSettings.ProductsEndpoint;
                _logger.LogDebug("Requesting products from: {Url}", productsUrl);

                // Add timeout
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));
                var response = await _httpClient.GetAsync(productsUrl, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("API request failed with status: {StatusCode}", response.StatusCode);
                    throw new HttpRequestException($"API returned {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Received {Length} characters of JSON data", json.Length);

                var productsResponse = JsonSerializer.Deserialize<ProductsResponseDto>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (productsResponse?.Products == null)
                {
                    _logger.LogError("Deserialization failed - Products property is null");
                    throw new JsonException("Invalid API response structure - missing Products");
                }

                _logger.LogInformation("Successfully loaded {Count} products from API", productsResponse.Products.Count());
                return productsResponse.Products;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError("API request timed out after {Timeout} seconds", _apiSettings.TimeoutSeconds);
                throw new HttpRequestException("API request timed out");
            }
            catch (HttpRequestException)
            {
                _logger.LogError("Network error while fetching products");
                throw; // Re-throw to be handled by controller
            }
            catch (JsonException)
            {
                _logger.LogError("Failed to parse API response JSON");
                throw; // Re-throw to be handled by controller
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetProducts");
                throw;
            }
        }

        public async Task<IEnumerable<CategoryFromApiDto>> GetCategories()
        {
            try
            {
                var categoriesUrl = _apiSettings.BaseUrl.TrimEnd('/') + "/" + _apiSettings.CategoriesEndpoint;
                _logger.LogDebug("Requesting categories from: {Url}", categoriesUrl);
                
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(_apiSettings.TimeoutSeconds));
                var response = await _httpClient.GetAsync(categoriesUrl, cts.Token);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("API request failed with status: {StatusCode}", response.StatusCode);
                    throw new HttpRequestException($"API returned {response.StatusCode}");
                }

                var json = await response.Content.ReadAsStringAsync();
                _logger.LogDebug("Received {Length} characters of JSON data", json.Length);

                var categories = JsonSerializer.Deserialize<List<CategoryFromApiDto>>(json, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (categories == null)
                {
                    _logger.LogError("Deserialization failed - categories is null");
                    throw new JsonException("Invalid API response structure for categories");
                }

                _logger.LogInformation("Successfully loaded {Count} categories from API", categories.Count);
                return categories;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError("API request timed out after {Timeout} seconds", _apiSettings.TimeoutSeconds);
                throw new HttpRequestException("API request timed out");
            }
            catch (HttpRequestException)
            {
                _logger.LogError("Network error while fetching categories");
                throw;
            }
            catch (JsonException)
            {
                _logger.LogError("Failed to parse API response JSON");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in GetCategories");
                throw;
            }
        }
    }
}
