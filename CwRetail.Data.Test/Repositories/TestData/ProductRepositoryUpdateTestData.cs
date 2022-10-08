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
    public class ProductRepositoryUpdateTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 0, new { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 1, 5, new { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 0, 5, new { Name = string.Empty, Price = 0M, Type = 0, Active = false } };
            yield return new object[] { 0, 5, new { Name = string.Empty } };
            yield return new object[] { 0, 5, new { Price = 0M } };
            yield return new object[] { 0, 5, new { Type = 0 } };
            yield return new object[] { 1, 5, new { Active = false } };
            yield return new object[] { 1, 5, new { Name = "Scooter" } };
            yield return new object[] { 1, 5, new { Price = 0.1M } };
            yield return new object[] { 1, 5, new { Type = (ProductTypeEnum)5 } };
            yield return new object[] { 1, 5, new { Active = true } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
