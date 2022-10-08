using Autofac.Extras.Moq;
using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using CwRetail.Data.Test.Repositories.TestData;
using CwRetail.Data.Test.Repositories.TestHelpers;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using Moq;
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

        [Fact]
        public void GetMockTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Mock<IProductRepository>()
                    .Setup(x => x.Get())
                    .Returns(ProductRepositoryTestHelper.GetSampleProducts());

                var repo = mock.Create<IProductRepository>();

                var expected = ProductRepositoryTestHelper.GetSampleProducts();
                var actual = repo.Get().ToList();

                Assert.True((actual != null) && (actual?.Count > 0));

                Assert.Equal(expected.Count, actual?.Count);

                Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
            }
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryInsertTestData))]
        public void InsertTest(int expectedNumberOfProductsInserted, Product product)
        {
            int numberOfProductsInserted = _repo.Insert(product);

            Assert.Equal(expectedNumberOfProductsInserted, numberOfProductsInserted);
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryInsertTestData))]
        public void InsertMockTest(int expectedNumberOfProductsInserted, Product product)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mocked = mock.Mock<IProductRepository>();

                mocked
                    .Setup(x => x.Insert(product))
                    .Returns(expectedNumberOfProductsInserted);

                var repo = mock.Create<IProductRepository>();

                var expected = expectedNumberOfProductsInserted;
                var actual = repo.Insert(product);

                mocked.Verify(x => x.Insert(product), Times.Exactly(1));

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryUpdateTestData))]
        public void UpdateTest(int expectedNumberOfProductsUpdated, long testId, dynamic product)
        {
            int numberOfProductsUpdated = _repo.Update(testId, JsonConvert.SerializeObject(product));

            Assert.Equal(expectedNumberOfProductsUpdated, numberOfProductsUpdated);
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryUpdateTestData))]
        public void UpdateMockTest(int expectedNumberOfProductsUpdated, long testId, dynamic product)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mocked = mock.Mock<IProductRepository>();

                string productAsString = JsonConvert.SerializeObject(product);

                mocked
                    .Setup(x => x.Update(testId, productAsString))
                    .Returns(expectedNumberOfProductsUpdated);

                var repo = mock.Create<IProductRepository>();

                var expected = expectedNumberOfProductsUpdated;
                var actual = repo.Update(testId, productAsString);

                mocked.Verify(x => x.Update(testId, productAsString), Times.Exactly(1));

                Assert.Equal(expected, actual);
            }
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryDeleteTestData))]
        public void DeleteTest(int expectedNumberOfProductsDeleted, long testId)
        {
            int numberOfProductsDeleted = _repo.Delete(testId);

            Assert.Equal(expectedNumberOfProductsDeleted, numberOfProductsDeleted);
        }

        [Theory]
        [ClassData(typeof(ProductRepositoryDeleteTestData))]
        public void DeleteMockTest(int expectedNumberOfProductsDeleted, long testId)
        {
            using (var mock = AutoMock.GetLoose())
            {
                var mocked = mock.Mock<IProductRepository>();

                mocked
                    .Setup(x => x.Delete(testId))
                    .Returns(expectedNumberOfProductsDeleted);

                var repo = mock.Create<IProductRepository>();

                var expected = expectedNumberOfProductsDeleted;
                var actual = repo.Delete(testId);

                mocked.Verify(x => x.Delete(testId), Times.Exactly(1));

                Assert.Equal(expected, actual);
            }
        }
    }
}
