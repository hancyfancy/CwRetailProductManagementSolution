using CwRetail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Repositories.Interface
{
    public interface IProductRepository
    {
        IEnumerable<Product> Get();

        int Insert(Product product);

        int Update(long id, string product);

        int Delete(long id);
    }
}
