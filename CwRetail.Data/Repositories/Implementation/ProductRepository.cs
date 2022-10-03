using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Implementation
{
    public class ProductRepository : IProductRepository
    {
        private readonly string _connectionString;

        public ProductRepository(string connectionString)
        { 
            _connectionString = connectionString;
        }

        public IEnumerable<Product> Get()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                {
                    connection.Open();
                    string sql = $@"SELECT 
	                                    p.Name, 
	                                    p.Price, 
	                                    p.Type, 
	                                    p.Active
                                    FROM 
	                                    production.products p";
                    return connection.Query<Product>(sql);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
