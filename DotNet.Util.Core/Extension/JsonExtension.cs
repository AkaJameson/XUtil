using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNet.Util.Core.Extension
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


    }
}
