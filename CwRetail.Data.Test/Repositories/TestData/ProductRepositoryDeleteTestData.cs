using CwRetail.Data.Enumerations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Test.Repositories.TestData
{
    public class ProductRepositoryDeleteTestData : IEnumerable<object[]>
    {
        private long _idOfProductToDelete;

        public ProductRepositoryDeleteTestData()
        {
            _idOfProductToDelete = 5;
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, -1 };
            yield return new object[] { 0, 0 };
            yield return new object[] { 1, _idOfProductToDelete };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
