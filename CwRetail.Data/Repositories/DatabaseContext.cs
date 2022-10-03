using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories
{
    public class DatabaseContext
    {
        private readonly IProductRepository _products;

        public DatabaseContext(string connectionString)
        {
            _products = new ProductRepository(connectionString);
        }

        public IProductRepository Products
        { 
            get { return _products; }
        }
    }
}
