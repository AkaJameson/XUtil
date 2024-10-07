using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
namespace XUtil.Core.Extension
{
    public static class JsonExtension
    {
        public static T ConvertToObject<T>(this string content) where T : class
        {
           return JsonConvert.DeserializeObject<T>(content);
        }

        public static string ConvertTostring<T>(this T obj) where T: class
        {
            return JsonConvert.SerializeObject(obj);
        }
        /// <summary>
        /// 将字典转换为 JSON 字符串
        /// </summary>
        /// <param name="dictionary"></param>
        /// <returns></returns>
        public static string ConvertDictionaryToString(this IDictionary<string, object> dictionary)
        {
            return JsonConvert.SerializeObject(dictionary);
        }
        /// <summary>
        /// 解析 JSON 字符串为 JObject
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static JObject ParseToJObject(this string json)
        {
            return JObject.Parse(json);
        }

        /// <summary>
        /// 从 JSON 字符串中获取特定属性值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public static T GetValue<T>(this string json, string propertyName)
        {
            var jObject = JObject.Parse(json);
            return jObject[propertyName].ToObject<T>();
        }
        /// <summary>
        /// 从 JSON 字符串转换为字典
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IDictionary<string, object> ConvertToDictionary(this string json)
        {
            return JsonConvert.DeserializeObject<IDictionary<string, object>>(json);
        }

    }
}
