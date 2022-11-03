using Autofac.Extras.Moq;
using CwRetail.Data.Extensions;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Interface;
using CwRetail.Data.Test.Extensions.TestData;
using CwRetail.Data.Test.Repositories.TestData;
using GenConversion.Service.Utilities.Implementation;
using GenConversion.Service.Utilities.Interface;
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
        private readonly ISqlConverter _sqlConverter;

        public DatabaseExtensionTests()
        {
            _sqlConverter = new JsonToSqlUpdateParameterConverter();
        }

        [Theory]
        [ClassData(typeof(DatabaseExtensionAsUpdateSqlTestData))]
        public void AsUpdateSqlTest(string expectedOutput, string input)
        {
            string output = _sqlConverter.ToSql(input);

            Assert.Equal(expectedOutput, output);
        }
    }
}
