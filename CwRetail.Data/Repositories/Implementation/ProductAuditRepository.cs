using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Implementation
{
    public class ProductAuditRepository : IProductAuditRepository
    {
        private readonly SqlConnection _connection;

        public ProductAuditRepository()
        {
            _connection = new SqlConnection(ConnectionStrings.Test);
        }

        public IEnumerable<ProductAudit> GetUpdates(long productId)
        {
            try
            {
                _connection.Open();

                string sql = $@"SELECT 
                                    p.ProductAuditId,
	                                p.ObjJson,
	                                p.AuditDateTime
                                FROM 
	                                audit.products p
                                WHERE
	                                p.EventType = 'UPDATE'
									AND p.ProductId = @ProductId
								ORDER BY 
									p.AuditDateTime";
                var result = _connection.Query<ProductAudit>(sql, new
                {
                    ProductId = productId
                });

                _connection.Close();

                return result;
            }
            catch (Exception e)
            {
                return new List<ProductAudit>();
            }
        }
    }
}
