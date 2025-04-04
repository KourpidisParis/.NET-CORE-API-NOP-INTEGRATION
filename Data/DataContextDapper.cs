using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using ErpConnector.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace ErpConnector.Data
{
    class DataContextDapper
    {
        private readonly IConfiguration _config;

        public DataContextDapper(IConfiguration config)
        {
            _config = config;
        }

        private IDbConnection CreateConnection()
        {
            return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
        }

        public IEnumerable<Product> GetProducts()
        {
            // using var connection = CreateConnection();
            using var connection = CreateConnection();
            string sql = "SELECT TOP 10 Id, Name, Sku, Price FROM [nop].[dbo].[Product]";
            return connection.Query<Product>(sql).ToList();
        }
    }
}
