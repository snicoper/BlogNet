using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;

namespace Web.Extensions
{
    public static class TempDataExtensions
    {
        /// <summary>
        /// Serializa objetos para TempData.
        /// </summary>
        public static void Put<T>(this ITempDataDictionary tempData, string key, T value)
            where T : class
        {
            tempData[key] = JsonConvert.SerializeObject(value);
        }

        /// <summary>
        /// Deserializa objetos para TempData.
        /// <see cref="ControllerExtensions.AddMessage(Microsoft.AspNetCore.Mvc.Controller, string, string)"/>
        /// </summary>
        public static T Get<T>(this ITempDataDictionary tempData, string key)
            where T : class
        {
            tempData.TryGetValue(key, out var o);
            return o == null ? null : JsonConvert.DeserializeObject<T>((string)o);
        }
    }
}
