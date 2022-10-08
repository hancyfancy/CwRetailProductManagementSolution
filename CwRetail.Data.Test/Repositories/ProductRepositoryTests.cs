using Autofac.Extras.Moq;
using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using CwRetail.Data.Test.Repositories.TestData;
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
                    .Returns(GetSampleProducts());

                var repo = mock.Create<IProductRepository>();

                var expected = GetSampleProducts();
                var actual = repo.Get().ToList();

                Assert.True((actual != null) && (actual?.Count > 0));

                Assert.Equal(expected.Count, actual?.Count);

                Assert.Equal(JsonConvert.SerializeObject(expected), JsonConvert.SerializeObject(actual));
            }
        }

        private List<Product> GetSampleProducts()
        {
            return new List<Product>() { 
                new Product()
                { 
                    Id = 3,
                    Name = "34107476-E228-4AD1-9C04-46ECA69BDE92",
                    Price = 1913.57M,
                    Type = ProductTypeEnum.Food,
                    Active = true,
                },
                new Product()
                { 
                    Id = 4,
                    Name = "AE8D7304-DB95-4D25-B04F-D3D36570990A",
                    Price = 1193.22M,
                    Type = ProductTypeEnum.Furniture,
                    Active = false,
                }
            };
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

        [Theory]
        [ClassData(typeof(ProductRepositoryDeleteTestData))]
        public void DeleteTest(int expectedNumberOfProductsDeleted, long testId)
        {
            int numberOfProductsDeleted = _repo.Delete(testId);

            Assert.Equal(expectedNumberOfProductsDeleted, numberOfProductsDeleted);
        }
    }
}
