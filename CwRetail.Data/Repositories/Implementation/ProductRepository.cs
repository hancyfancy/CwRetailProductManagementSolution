using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
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
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
