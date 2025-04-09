using ErpConnector.Data;
using ErpConnector.Repository.IRepository;
using ErpConnector.Models;

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

        public async Task InsertProduct(Product product)
        {
            string sql = @"
                INSERT INTO [nop].[dbo].[Product] (Name, ShortDescription, Price,ProductTypeId ,Published, CreatedOnUtc, ApiId)
                VALUES (@Name, @Description, @Price, 1,@ProductTypeId ,GETUTCDATE(), @ApiId);";

            _dapper.Execute(sql, new
            {
                Name = product.Name,
                Description = product.FullDescription,
                Price = product.Price,
                ApiId = product.ApiId,
                ProductTypeId = product.ProductTypeId
            });

            await Task.CompletedTask;
        }

        public async Task UpdateProduct(Product product, int id)
        {
            string sql = @"
                UPDATE [nop].[dbo].[Product]
                SET Name = @Name,
                    ShortDescription = @Description,
                    Price = @Price
                WHERE Id = @Id;";

            _dapper.Execute(sql, new
            {
                Name = product.Name,
                Description = product.FullDescription,
                Price = product.Price,
                Id = id
            });

            await Task.CompletedTask;
        }
    }
}