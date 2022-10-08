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
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, -1 };
            yield return new object[] { 0, 0 };
            yield return new object[] { 1, 4 };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
