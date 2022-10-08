using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using CwRetail.Data.Test.Repositories.TestData;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Test.Repositories
{
    public class ProductRepositoryTests
    {
        private IProductRepository _repo;

        public ProductRepositoryTests()
        { 
            _repo = new ProductRepository(ConnectionStrings.Test);
        }

        [Fact]
        public void GetTest()
        {
            List<Product> products = _repo.Get().ToList();

            Assert.IsType<List<Product>>(products);
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryInsertTestData))]
        public void InsertTest(int expectedNumberOfProductsInserted, Product product)
        {
            int numberOfProductsInserted = _repo.Insert(product);

            Assert.Equal(expectedNumberOfProductsInserted, numberOfProductsInserted);
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryUpdateTestData))]
        public void UpdateTest(int expectedNumberOfProductsUpdated, long testId, dynamic product)
        {
            int numberOfProductsUpdated = _repo.Update(testId, JsonConvert.SerializeObject(product));

            Assert.Equal(expectedNumberOfProductsUpdated, numberOfProductsUpdated);
        }
    }
}
