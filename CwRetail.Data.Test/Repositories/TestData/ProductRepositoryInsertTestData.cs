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
            yield return new object[] { 0, new Product() { Name = default, Price = default, Type = default, Active = default } };
            yield return new object[] { 1, new Product() { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 0, new Product() { Name = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 0, new Product() { Name = "Scooter", Price = -10.39M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 0, new Product() { Name = "Scooter", Price = 0M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 0, new Product() { Name = "Scooter", Price = 25.50M, Type = 0, Active = true } };
            yield return new object[] { 0, new Product() { Name = "Scooter", Price = 25.50M, Type = (ProductTypeEnum)6, Active = true } };
            yield return new object[] { 1, new Product() { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = false } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
