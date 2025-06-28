using ErpConnector.Data;
using Microsoft.Extensions.Logging;
using Microsoft.Data.SqlClient;

namespace ErpConnector.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ILogger<DbInitializer> _logger;
        private readonly DataContextDapper _dapper;

        public DbInitializer(ILogger<DbInitializer> logger, DataContextDapper dapper)
        {
            _logger = logger;
            _dapper = dapper;
        }

        public async Task InitializeProductTableAsync()
        {
            try
            {
                _logger.LogInformation("Checking Product table for ApiId column");
                
                var columnExists = await _dapper.LoadDataSingleAsync<int>(
                    @"SELECT COUNT(*) 
                      FROM INFORMATION_SCHEMA.COLUMNS 
                      WHERE TABLE_NAME = 'Product' AND COLUMN_NAME = 'ApiId'");

                if (columnExists == 0)
                {
                    _logger.LogInformation("Adding 'ApiId' column to Product table");
                    Console.WriteLine("Adding 'ApiId' column to Product table...");

                    string alterTableSql = @"
                        ALTER TABLE [nop].[dbo].[Product]
                        ADD ApiId VARCHAR(255) NULL;

                        ALTER TABLE [nop].[dbo].[Product]
                        ADD CONSTRAINT UQ_Product_ApiId UNIQUE (ApiId);";

                    await _dapper.ExecuteAsync(alterTableSql);
                    _logger.LogInformation("'ApiId' column added successfully to Product table");
                    Console.WriteLine("'ApiId' column added.");
                }
                else
                {
                    _logger.LogDebug("'ApiId' column already exists in Product table");
                    Console.WriteLine("'ApiId' column already exists.");
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error during Product table initialization");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Product table initialization");
                throw;
            }
        }

        public async Task InitializeCategoryTableAsync()
        {
            try
            {
                _logger.LogInformation("Checking Category table for ApiId column");
                
                var columnExists = await _dapper.LoadDataSingleAsync<int>(
                    @"SELECT COUNT(*) 
                      FROM INFORMATION_SCHEMA.COLUMNS 
                      WHERE TABLE_NAME = 'Category' AND COLUMN_NAME = 'ApiId'");

                if (columnExists == 0)
                {
                    _logger.LogInformation("Adding 'ApiId' column to Category table");
                    Console.WriteLine("Adding 'ApiId' column to Category table...");

                    string alterTableSql = @"
                        ALTER TABLE [nop].[dbo].[Category]
                        ADD ApiId VARCHAR(255) NULL;

                        ALTER TABLE [nop].[dbo].[Category]
                        ADD CONSTRAINT UQ_Category_ApiId UNIQUE (ApiId);";

                    await _dapper.ExecuteAsync(alterTableSql);
                    _logger.LogInformation("'ApiId' column added successfully to Category table");
                    Console.WriteLine("'ApiId' column added.");
                }
                else
                {
                    _logger.LogDebug("'ApiId' column already exists in Category table");
                    Console.WriteLine("'ApiId' column already exists.");
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error during Category table initialization");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during Category table initialization");
                throw;
            }
        }
    }
}
