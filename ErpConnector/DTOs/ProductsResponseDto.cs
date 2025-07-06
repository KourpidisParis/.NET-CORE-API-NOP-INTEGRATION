using ErpConnector.DTOs;

namespace ErpConnector
{
    public class ProductsResponseDto
    {
        public List<ProductFromApiDto>? Products { get; set; }
        public int Total { get; set; }
        public int Skip { get; set; }
        public int Limit { get; set; }
    }
}