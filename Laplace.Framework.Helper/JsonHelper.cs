using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Laplace.Framework.Helper
{
    /// <summary>  
    /// JSON序列化和反序列化辅助类  
    /// </summary>  
    public static class JsonHelper
    {

        /// <summary>  
        /// JSON序列化  
        /// </summary>  
        public static string SerializerJson<T>(this T t)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream();
            ser.WriteObject(ms, t);
            string jsonString = Encoding.UTF8.GetString(ms.ToArray());
            ms.Close();
            return jsonString;
        }

        /// <summary>  
        /// JSON反序列化  
        /// </summary>  
        public static T DeserializeJson<T>(this string jsonString)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));
            T obj = (T)ser.ReadObject(ms);
            return obj;
        }

        public static string ToJson(this object obj)
        {
            return NewtonsoftJson(obj);
        }
        public static string NewtonsoftJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None);
        }
    }
}
