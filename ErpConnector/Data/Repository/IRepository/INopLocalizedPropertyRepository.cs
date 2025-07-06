using ErpConnector.Models;

namespace ErpConnector.Repository.IRepository
{
    public interface INopLocalizedPropertyRepository
    {
        Task<IEnumerable<LocalizedProperty>> GetAllLocalizedProperties();
        Task<int> InsertLocalizedProperty(LocalizedProperty localizedProperty);
        Task UpdateLocalizedProperty(int id, string localeValue);
        Task<int?> GetLocalizedPropertyId(string localeKeyGroup, string localeKey, int entityId, int languageId);
    }
}