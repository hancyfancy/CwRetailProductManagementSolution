using Newtonsoft.Json;

namespace CwRetail.Api.Extensions
{
    public static class JsonExtensions
    {
        public static string ToJson<T>(this T obj)
        {
            try
            {
                return JsonConvert.SerializeObject(obj);
            }
            catch (Exception e)
            {
                return default;
            }
        }

        public static T ToObj<T>(this string json)
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception e)
            {
                return default;
            }
        }
    }
}
