using ErpConnector.Models;

namespace ErpConnector.Processors.IProcessor
{
    public interface IProductProcessor
    {
        Product ApplyDefaultProductValues(Product product);
    }
}
