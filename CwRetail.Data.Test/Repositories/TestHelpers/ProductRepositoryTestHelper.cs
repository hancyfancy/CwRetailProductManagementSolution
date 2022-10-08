using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Test.Repositories.TestHelpers
{
    public static class ProductRepositoryTestHelper
    {
        public static List<Product> GetSampleProducts()
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
    }
}
