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
        private long _idOfProductToUpdate;

        public ProductRepositoryUpdateTestData()
        {
            _idOfProductToUpdate = 6;   
        }

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { 0, 0, new { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 1, _idOfProductToUpdate, new { Name = "Scooter", Price = 25.50M, Type = ProductTypeEnum.Toys, Active = true } };
            yield return new object[] { 0, _idOfProductToUpdate, new { Name = string.Empty, Price = 0M, Type = 0, Active = false } };
            yield return new object[] { 0, _idOfProductToUpdate, new { Name = string.Empty } };
            yield return new object[] { 0, _idOfProductToUpdate, new { Price = 0M } };
            yield return new object[] { 0, _idOfProductToUpdate, new { Type = 0 } };
            yield return new object[] { 1, _idOfProductToUpdate, new { Active = false } };
            yield return new object[] { 1, _idOfProductToUpdate, new { Name = "Scooter" } };
            yield return new object[] { 1, _idOfProductToUpdate, new { Price = 0.1M } };
            yield return new object[] { 1, _idOfProductToUpdate, new { Type = (ProductTypeEnum)5 } };
            yield return new object[] { 1, _idOfProductToUpdate, new { Active = true } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
