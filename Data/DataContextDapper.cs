using System.Data;
using Dapper;
using ErpConnector.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ErpConnector.Data
{
    public class DataContextDapper
    {
        private readonly string _connectionString;

        public DataContextDapper(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        private async Task<SqlConnection> CreateConnectionAsync()
        {
            var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();
            return connection;
        }

        public async Task<T?> LoadDataSingleAsync<T>(string sql, object? parameters = null)
        {
            await using var connection = await CreateConnectionAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> LoadDataAsync<T>(string sql, object? parameters = null)
        {
            await using var connection = await CreateConnectionAsync();
            return await connection.QueryAsync<T>(sql, parameters);
        }

        public async Task<bool> ExecuteAsync(string sql, object? parameters = null)
        {
            await using var connection = await CreateConnectionAsync();
            var rowsAffected = await connection.ExecuteAsync(sql, parameters);
            return rowsAffected > 0;
        }

        public async Task<IEnumerable<Product>> GetProductsAsync()
        {
            const string sql = "SELECT TOP 10 Id, Name, Sku, Price FROM [nop].[dbo].[Product]";
            await using var connection = await CreateConnectionAsync();
            return await connection.QueryAsync<Product>(sql);
        }
    }
}
