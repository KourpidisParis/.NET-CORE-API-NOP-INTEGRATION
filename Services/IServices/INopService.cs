using ErpConnector.DTOs;

namespace ErpConnector.Services.IServices
{
    public interface INopService
    {
        Task SyncProducts(IEnumerable<ProductFromApiDto> products);
    }
}
