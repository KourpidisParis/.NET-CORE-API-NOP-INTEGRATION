using ErpConnector.DTOs;

namespace ErpConnector.Services.IServices
{
    public interface INopProductService
    {
        Task SyncProducts(IEnumerable<ProductFromApiDto> products);
    }
}
