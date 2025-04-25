using ErpConnector.Data;

namespace ErpConnector.Services
{
    public class DbInitializer : IDbInitializer
    {
        private readonly DataContextDapper _dapper;

        public DbInitializer(DataContextDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task InitializeProductTableAsync()
        {
            var columnExists = _dapper.LoadDataSingle<int>(
                @"SELECT COUNT(*) 
                  FROM INFORMATION_SCHEMA.COLUMNS 
                  WHERE TABLE_NAME = 'Product' AND COLUMN_NAME = 'ApiId'");

            if (columnExists == 0)
            {
                Console.WriteLine("Adding 'ApiId' column to Product table...");

                string alterTableSql = @"
                    ALTER TABLE [nop].[dbo].[Product]
                    ADD ApiId VARCHAR(255) NULL;

                    ALTER TABLE [nop].[dbo].[Product]
                    ADD CONSTRAINT UQ_Product_ApiId UNIQUE (ApiId);";

                _dapper.Execute(alterTableSql);
                Console.WriteLine("'ApiId' column added.");
            }
            else
            {
                Console.WriteLine("'ApiId' column already exists.");
            }

            await Task.CompletedTask;
        }

        public async Task InitializeCategoryTableAsync()
        {
            var columnExists = _dapper.LoadDataSingle<int>(
                @"SELECT COUNT(*) 
                  FROM INFORMATION_SCHEMA.COLUMNS 
                  WHERE TABLE_NAME = 'Category' AND COLUMN_NAME = 'ApiId'");

            if (columnExists == 0)
            {
                Console.WriteLine("Adding 'ApiId' column to Category table...");

                string alterTableSql = @"
                    ALTER TABLE [nop].[dbo].[Category]
                    ADD ApiId VARCHAR(255) NULL;

                    ALTER TABLE [nop].[dbo].[Category]
                    ADD CONSTRAINT UQ_Category_ApiId UNIQUE (ApiId);";

                _dapper.Execute(alterTableSql);
                Console.WriteLine("'ApiId' column added.");
            }
            else
            {
                Console.WriteLine("'ApiId' column already exists.");
            }

            await Task.CompletedTask;
        }
    }
}
