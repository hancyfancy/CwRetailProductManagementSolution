using Autofac.Extras.Moq;
using CwRetail.Data.Extensions;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using CwRetail.Data.Test.Extensions.TestData;
using CwRetail.Data.Test.Repositories.TestData;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Test.Extensions
{
    public class DatabaseExtensionTests
    {
        public DatabaseExtensionTests()
        { 
            
        }

        [Theory]
        [ClassData(typeof(DatabaseExtensionAsUpdateSqlTestData))]
        public void AsUpdateSqlTest(string expectedOutput, string input)
        {
            string output = input.AsUpdateSql();

            Assert.Equal(expectedOutput, output);
        }
    }
}
