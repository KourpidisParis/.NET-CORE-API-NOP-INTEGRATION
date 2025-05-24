using ErpConnector.Data;
using ErpConnector.Repository.IRepository;
using ErpConnector.Models;

namespace ErpConnector.Repository
{
    public class NopLocalizedPropertyRepository : INopLocalizedPropertyRepository
    {
        private readonly DataContextDapper _dapper;
        public NopLocalizedPropertyRepository(DataContextDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<IEnumerable<LocalizedProperty>> GetAllLocalizedProperties()
        {
            string sql = @"
                SELECT *
                FROM [nop].[dbo].[LocalizedProperty]";
            
            var localizedProperties = _dapper.LoadData<LocalizedProperty>(sql);
            
            return await Task.FromResult(localizedProperties);
        }

        public async Task<int> InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[LocalizedProperty]
                (LocaleKeyGroup, LocaleKey, LocaleValue, LanguageId, EntityId)
                VALUES
                (@LocaleKeyGroup, @LocaleKey, @LocaleValue, @LanguageId, @EntityId);
                SELECT CAST(SCOPE_IDENTITY() AS INT)";
            
            var newId = _dapper.LoadDataSingle<int>(sql, localizedProperty);
            
            return await Task.FromResult(newId);
        }

        public async Task UpdateLocalizedProperty(int id, string localeValue)
        {
            string sql = @"
                UPDATE [nop].[dbo].[LocalizedProperty]
                SET LocaleValue = @LocaleValue
                WHERE Id = @Id";
            
            _dapper.Execute(sql, new { Id = id, LocaleValue = localeValue });
            
            await Task.CompletedTask;
        }

        public async Task<int?> GetLocalizedPropertyId(string localeKeyGroup, string localeKey, int entityId, int languageId)
        {
            string sql = @"
                SELECT Id FROM [nop].[dbo].[LocalizedProperty]
                WHERE LocaleKeyGroup = @LocaleKeyGroup
                AND LocaleKey = @LocaleKey
                AND EntityId = @EntityId
                AND LanguageId = @LanguageId";
            
            var id = _dapper.LoadDataSingle<int?>(sql, new { 
                LocaleKeyGroup = localeKeyGroup,
                LocaleKey = localeKey,
                EntityId = entityId,
                LanguageId = languageId
            });
            
            return await Task.FromResult(id);
        }
    }
}