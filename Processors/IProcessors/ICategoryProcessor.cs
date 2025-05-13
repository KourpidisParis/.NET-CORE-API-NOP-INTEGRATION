using ErpConnector.DTOs;
using ErpConnector.Models;

namespace ErpConnector.Processors.IProcessor
{
    public interface ICategoryProcessor
    {
        Category Map(CategoryFromApiDto categoryDto);
    }
}
