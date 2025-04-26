using ErpConnector.Models;

namespace ErpConnector.Processors.IProcessor
{
    public interface ICategoryProcessor
    {
        Category ApplyDefaultCategoryValues(Category category);
    }
}
