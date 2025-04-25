using ErpConnector.Data;
using ErpConnector.Models;
using ErpConnector.Repository.IRepository;

namespace ErpConnector.Repository
{
    public class NopCategoryRepository : INopCategoryRepository
    {
        private readonly DataContextDapper _dapper;

        public NopCategoryRepository(DataContextDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int?> GetCategoryIdByExternalId(string apiId)
        {
            var id = _dapper.LoadDataSingle<int?>(
                "SELECT TOP 1 Id FROM [nop].[dbo].[Category] WHERE ApiId = @ApiId",
                new { ApiId = apiId });

            return await Task.FromResult(id);
        }

        public async Task InsertCategory(Category category)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[Category] (Name, Description, ApiId)
                VALUES (@Name, @Description, @ApiId);";

            _dapper.Execute(sql, category);
            await Task.CompletedTask;
        }

        public async Task UpdateCategory(Category category, int id)
        {
            string sql = @"
                UPDATE [nop].[dbo].[Category]
                SET Name = @Name,
                    Description = @Description
                WHERE Id = @Id;";

            _dapper.Execute(sql, new
            {
                category.Name,
                category.Description,
                Id = id
            });

            await Task.CompletedTask;
        }
    }
}
