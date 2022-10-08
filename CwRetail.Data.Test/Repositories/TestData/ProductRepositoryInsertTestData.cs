using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Test.Repositories.TestData
{
    public class ProductRepositoryInsertTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new Product() { Name = default, Price = default, Type = default, Active = default }, 0 };
            //yield return new object[] { new Product() { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true }, 1 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
