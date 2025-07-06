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
            
            return await _dapper.LoadDataAsync<LocalizedProperty>(sql);            
        }

        public async Task<int> InsertLocalizedProperty(LocalizedProperty localizedProperty)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[LocalizedProperty]
                (LocaleKeyGroup, LocaleKey, LocaleValue, LanguageId, EntityId)
                VALUES
                (@LocaleKeyGroup, @LocaleKey, @LocaleValue, @LanguageId, @EntityId);
                SELECT CAST(SCOPE_IDENTITY() AS INT)";
            
            return await _dapper.LoadDataSingleAsync<int>(sql, localizedProperty);            
        }

        public async Task UpdateLocalizedProperty(int id, string localeValue)
        {
            string sql = @"
                UPDATE [nop].[dbo].[LocalizedProperty]
                SET LocaleValue = @LocaleValue
                WHERE Id = @Id";
            
            await _dapper.ExecuteAsync(sql, new { Id = id, LocaleValue = localeValue });
        }

        public async Task<int?> GetLocalizedPropertyId(string localeKeyGroup, string localeKey, int entityId, int languageId)
        {
            string sql = @"
                SELECT Id FROM [nop].[dbo].[LocalizedProperty]
                WHERE LocaleKeyGroup = @LocaleKeyGroup
                AND LocaleKey = @LocaleKey
                AND EntityId = @EntityId
                AND LanguageId = @LanguageId";
            
            return await _dapper.LoadDataSingleAsync<int?>(sql, new { 
                LocaleKeyGroup = localeKeyGroup,
                LocaleKey = localeKey,
                EntityId = entityId,
                LanguageId = languageId
            });            
        }
    }
}