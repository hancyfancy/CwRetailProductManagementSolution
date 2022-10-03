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
        private static readonly IProductRepository _products;

        static DatabaseContext()
        {
            _products = new ProductRepository();
        }

        public static IProductRepository Products
        { 
            get { return _products; }
        }
    }
}
