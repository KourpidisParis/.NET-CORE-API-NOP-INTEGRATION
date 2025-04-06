using System.Data;
using Dapper;
using ErpConnector.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ErpConnector.Data
{
    public class DataContextDapper
    {
        private readonly IConfiguration _config;
        private readonly string _connectionString;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
            _connectionString = _config.GetConnectionString("DefaultConnection");
        }

        public T LoadDataSingle<T>(string sql, object? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.QueryFirstOrDefault<T>(sql, parameters);
        }

        public IEnumerable<T> LoadData<T>(string sql, object? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Query<T>(sql, parameters);
        }

        public bool Execute(string sql, object? parameters = null)
        {
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            return dbConnection.Execute(sql, parameters) > 0;
        }
        public IEnumerable<Product> GetProducts()
        {
            // using var connection = CreateConnection();
            using IDbConnection dbConnection = new SqlConnection(_connectionString);
            string sql = "SELECT TOP 10 Id, Name, Sku, Price FROM [nop].[dbo].[Product]";
            return dbConnection.Query<Product>(sql).ToList();
        }
    }
}
