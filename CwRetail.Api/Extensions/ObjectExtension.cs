using System.Dynamic;

namespace CwRetail.Api.Extensions
{
    public static class ObjectExtension
    {
        public static dynamic WithPopulatedProperties<T>(this T objectToTransform)
        {
            if (objectToTransform is null)
            {
                return new { };
            }

            var type = objectToTransform.GetType();
            var returnClass = new ExpandoObject() as IDictionary<string, object>;
            foreach (var propertyInfo in type.GetProperties())
            {
                var value = propertyInfo.GetValue(objectToTransform);
                var valueIsNotAString = !(value is string && !string.IsNullOrWhiteSpace(value.ToString()));
                if (valueIsNotAString && value != null)
                {
                    returnClass.Add(propertyInfo.Name, value);
                }
            }
            return returnClass;
        }
    }
}
