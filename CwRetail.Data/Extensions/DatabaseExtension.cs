using CwRetail.Data.Enumerations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CwRetail.Data.Extensions
{
    public static class DatabaseExtension
    {
        public static string AsUpdateSql(this string json)
        {
            try
            {
                string output = string.Empty;

                string properties = json.Replace("{", "").Replace("}", "").Replace("\r\n", "").Trim();

                List<string> props = properties.Split(",").ToList();

                for (int i = 0; i < props.Count; i++)
                {
                    string prop = props[i];

                    List<string> kv = prop.Split(":").ToList();

                    if (i > 0)
                    {
                        output += ",";
                    }

                    //if (string.Equals(kv[0].Replace("\"", "").Trim(), "Type") && (!kv[1].Replace("\"", "").Trim().All(Char.IsLetter)))
                    //{
                    //    int enumInt = int.Parse(kv[1]);

                    //    kv[1] = "'" + ((ProductTypeEnum)enumInt).ToString() + "'";
                    //}

                    output += kv[0].Replace("\"", "") + "=" + kv[1].Replace("\"", "'").Replace("true", "1").Replace("false", "0");
                }

                return output;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
