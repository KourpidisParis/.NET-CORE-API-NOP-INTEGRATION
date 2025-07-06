using System.Data;
using Dapper;
using ErpConnector.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ErpConnector.Data
{
    public class DataContextDapper
    {
        private readonly string _connectionString;
        private readonly ILogger<DataContextDapper> _logger;

        public DataContextDapper(IConfiguration config, ILogger<DataContextDapper> logger)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            _logger = logger;
        }

        private async Task<SqlConnection> CreateConnectionAsync()
        {
            try
            {
                var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();
                _logger.LogDebug("Database connection opened successfully");
                return connection;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Failed to open database connection");
                throw;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Invalid connection string configuration");
                throw;
            }
        }

        public async Task<T?> LoadDataSingleAsync<T>(string sql, object? parameters = null)
        {
            try
            {
                _logger.LogDebug("Executing query: {Sql}", sql);
                await using var connection = await CreateConnectionAsync();
                var result = await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
                _logger.LogDebug("Query executed successfully, returned: {HasResult}", result != null);
                return result;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error executing query: {Sql}", sql);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error executing query: {Sql}", sql);
                throw;
            }
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql, object? parameters = null)
        {
            try
            {
                _logger.LogDebug("Executing query: {Sql}", sql);
                await using var connection = await CreateConnectionAsync();
                var results = await connection.QueryAsync<T>(sql, parameters);
                _logger.LogDebug("Query executed successfully, returned {Count} results", results.Count());
                return results;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error executing query: {Sql}", sql);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error executing query: {Sql}", sql);
                throw;
            }
        }

        public async Task<bool> ExecuteAsync(string sql, object? parameters = null)
        {
            try
            {
                _logger.LogDebug("Executing command: {Sql}", sql);
                await using var connection = await CreateConnectionAsync();
                var rowsAffected = await connection.ExecuteAsync(sql, parameters);
                _logger.LogDebug("Command executed successfully, {RowsAffected} rows affected", rowsAffected);
                return rowsAffected > 0;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error executing command: {Sql}", sql);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error executing command: {Sql}", sql);
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            try
            {
                const string sql = "SELECT TOP 10 Id, Name, Sku, Price FROM [nop].[dbo].[Product]";
                _logger.LogDebug("Fetching products with query: {Sql}", sql);
                await using var connection = await CreateConnectionAsync();
                var products = await connection.QueryAsync<Product>(sql);
                _logger.LogInformation("Retrieved {Count} products", products.Count());
                return products;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "SQL error fetching products");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error fetching products");
                throw;
            }
        }
    }
}
