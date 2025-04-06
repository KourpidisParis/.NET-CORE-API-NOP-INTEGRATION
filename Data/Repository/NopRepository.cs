using ErpConnector.DTOs;
using ErpConnector.Data;
using ErpConnector.Repository.IRepository;

namespace ErpConnector.Repository
{
    public class NopRepository : INopRepository
    {
        private readonly DataContextDapper _dapper;
        public NopRepository(DataContextDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task<int?> GetProductIdByExternalId(string apiId)
        {
           var id = _dapper.LoadDataSingle<int?>(
                "SELECT TOP 1 Id FROM [nop].[dbo].[Product] WHERE ApiId = @ApiId",
                new { ApiId = apiId });

            return await Task.FromResult(id);        
        }

        public async Task InsertProduct(ProductFromApiDto product)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[Product] (Name, ShortDescription, Price, Published, CreatedOnUtc, ApiId)
                VALUES (@Name, @Description, @Price, 1, GETUTCDATE(), @ApiId);";

            _dapper.Execute(sql, new
            {
                Name = product.Title,
                Description = product.Description,
                Price = product.Price,
                ApiId = product.Id
            });

            await Task.CompletedTask;
        }

        public async Task UpdateProduct(ProductFromApiDto product, int id)
        {
            string sql = @"
                UPDATE [nop].[dbo].[Product]
                SET Name = @Name,
                    ShortDescription = @Description,
                    Price = @Price
                WHERE Id = @Id;";

            _dapper.Execute(sql, new
            {
                Name = product.Title,
                Description = product.Description,
                Price = product.Price,
                Id = id
            });

            await Task.CompletedTask;
        }
    }
}