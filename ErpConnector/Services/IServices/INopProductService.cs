using ErpConnector.DTOs;
using ErpConnector.Models;

namespace ErpConnector.Services.IServices
{
    public interface INopProductService
    {
        Task SyncProducts(IEnumerable<ProductFromApiDto> products);
        Task MapProductToCategory(Product productModel, int? productId);
    }
}
