using CwRetail.Data.Enumerations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CwRetail.Data.Test.Extensions.TestData
{
    public class DatabaseExtensionAsUpdateSqlTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { Regex.Replace("Name= 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA',    Price= 54.35,    Type='Books',    Active= 0", @"\s+", ""), Regex.Replace("{\r\n    \"Name\": \"AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA\",\r\n    \"Price\": 54.35,\r\n    \"Type\": 1,\r\n    \"Active\": false\r\n}", @"\s+", "") };
            yield return new object[] { Regex.Replace("", @"\s+", ""), Regex.Replace("", @"\s+", "") };
            yield return new object[] { Regex.Replace("", @"\s+", ""), Regex.Replace("", @"\s+", "") };
            yield return new object[] { Regex.Replace("", @"\s+", ""), Regex.Replace("", @"\s+", "") };
            yield return new object[] { Regex.Replace("", @"\s+", ""), Regex.Replace("", @"\s+", "") };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
