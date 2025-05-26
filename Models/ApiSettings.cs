namespace ErpConnector.Models
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = "https://dummyjson.com/";
        public int TimeoutSeconds { get; set; } = 30;
        public string ProductsEndpoint { get; set; } = "products";
        public string CategoriesEndpoint { get; set; } = "products/categories";
    }
}