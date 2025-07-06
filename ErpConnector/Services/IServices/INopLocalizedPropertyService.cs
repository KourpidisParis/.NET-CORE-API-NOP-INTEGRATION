using ErpConnector.Models;

namespace ErpConnector.Services.IServices
{
    public interface INopLocalizedPropertyService
    {
        Task HandleLocalizedProperty(LocalizedProperty localizedProperty);
    }
}
