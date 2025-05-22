using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using VSW.Core.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VSW.Core
{
    public static class JsonHelper
    {
        public static T LoadFromFile<T>(string filePath) where T: new()
        {
            if (FileHelper.FileExists(filePath))
            {
                using (var sr = new StreamReader(filePath))
                {
                    string json = sr.ReadToEnd().TrimToEmpty();
                    if(json.IsEmpty())
                    {
                        return new T();
                    }

                    return Parse<T>(json);
                }
            }

            return new T();
        }

        public static JToken ToJToken(this object obj, JsonSerializerSettings setting = null)
        {
            if (setting != null)
            {
                var serializer = JsonSerializer.Create(setting);
                return JToken.FromObject(obj, serializer);
            }

            return JToken.FromObject(obj);
        }

        public static object ParseFromToken(this JToken obj, Type type, JsonSerializerSettings setting = null)
        {
            if (setting != null)
            {
                var serializer = JsonSerializer.Create(setting);
                return obj.ToObject(type, serializer);
            }

            return obj.ToObject(type);
        }

        public static T ParseFromToken<T>(this JToken obj, JsonSerializerSettings setting = null)
        {
            if (setting != null)
            {
                var serializer = JsonSerializer.Create(setting);
                return obj.ToObject<T>(serializer);
            }

            return obj.ToObject<T>();
        }

        public static JObject ToJObject(this object obj, JsonSerializerSettings setting = null)
        {
            if (setting != null)
            {
                var serializer = JsonSerializer.Create(setting);
                return JObject.FromObject(obj, serializer);
            }

            return JObject.FromObject(obj);
        }

        public static JObject ParseToJObject(this string data)
        {
            return JObject.Parse(data);
        }

        public static JToken ParseToJToken(this string data)
        {
            return JToken.Parse(data);
        }

        /// <summary>
        /// Chuyển json object sang kiểu chỉ định
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jObj"></param>
        /// <returns></returns>
        public static T ParseFromJObject<T>(this JObject jObj, JsonSerializerSettings setting = null)
        {
            if (setting != null)
            {
                var serializer = JsonSerializer.Create(setting);
                return jObj.ToObject<T>(serializer);
            }

            return jObj.ToObject<T>();
        }

        public static object ParseFromJObject(this JObject jObj, Type t, JsonSerializerSettings setting = null)
        {
            if (setting != null)
            {
                var serializer = JsonSerializer.Create(setting);
                return jObj.ToObject(t, serializer);
            }

            return jObj.ToObject(t);
        }

        public static bool TryParse<T>(string input, out T obj, bool strict = JsonSettings.UseStrictAsDefault)
        {
            obj = default;
            try
            {
                obj = Parse<T>(input, strict);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Deserializes a JSON string to an object
        /// </summary>
        /// <typeparam name="T">Type to deserialize</typeparam>
        /// <param name="input">JSON string</param>
        /// <returns>Deserialized object</returns>
        public static T Parse<T>(string input, bool strict = JsonSettings.UseStrictAsDefault)
        {
            var settings = strict ? JsonSettings.Strict : JsonSettings.Tolerant;
            return JsonConvert.DeserializeObject<T>(input, settings.Value);
        }

        /// <summary>
        /// Deserializes a JSON string to an object.
        /// </summary>
        /// <param name="targetType">Type to deserialize</param>
        /// <param name="input">JSON string</param>
        /// <param name="strict">true:Strict; fale: Tolerant</param>
        /// <returns>Deserialized object</returns>
        public static object Parse(string input, Type targetType, bool strict = JsonSettings.UseStrictAsDefault)
        {
            var settings = strict ? JsonSettings.Strict : JsonSettings.Tolerant;
            return JsonConvert.DeserializeObject(input, targetType, settings.Value);
        }

        public static bool TryParse(string input, Type targetType, out object obj, bool strict = JsonSettings.UseStrictAsDefault)
        {
            obj = null;
            try
            {
                obj = Parse(input, targetType, strict);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ToPrettyJson(this string json)
        {
            if(json.IsEmpty())
            {
                return json;
            }

            var jsonObject = ParseToJToken(json);
            return jsonObject.ToString(Formatting.Indented);
        }

        /// <summary>
        /// Converts object to its JSON representation
        /// </summary>
        /// <param name="value">Value to convert to JSON</param>
        /// <returns>Serialized JSON string</returns>
        public static string Stringify(object value, bool strict = JsonSettings.UseStrictAsDefault, bool format = false)
        {
            var settings = strict ? JsonSettings.Strict : JsonSettings.Tolerant;

            if (format)
            {
                return JsonConvert.SerializeObject(value, Formatting.Indented, settings.Value);
            }
            else
            {
                return JsonConvert.SerializeObject(value, settings.Value);
            }            
        }

        /// <summary>
        /// Converts object to its JSON representation
        /// </summary>
        /// <param name="value">Value to convert to JSON</param>
        /// <param name="indentation">Indentation (default 4)</param>
        /// <returns>Serialized JSON string</returns>
        public static string StringifyIndented(object value, int indentation = 4, bool strict = JsonSettings.UseStrictAsDefault)
        {
            var settings = strict ? JsonSettings.Strict : JsonSettings.Tolerant;
            using (var sw = new StringWriter())
            using (var jw = new JsonTextWriter(sw))
            {
                jw.Formatting = Formatting.Indented;
                jw.IndentChar = ' ';
                jw.Indentation = indentation;

                var serializer = JsonSerializer.Create(settings.Value);
                serializer.Serialize(jw, value);
                return sw.ToString();
            }
        }

        /// <summary>
        ///   Converts an object to its JSON representation (extension method for Stringify)</summary>
        /// <param name="value">
        ///   Object</param>
        /// <returns>
        ///   JSON representation string.</returns>
        /// <remarks>
        ///   null, Int32, Boolean, DateTime, Decimal, Double, Guid types handled automatically.
        ///   If object has a ToJson method it is used, otherwise value.ToString() is used as last fallback.</remarks>
        public static string ToJson(this object value, bool strict = JsonSettings.UseStrictAsDefault, bool format = false)
        {
            return Stringify(value, strict, format);
        }

        public static string SelectStringToken(this JObject obj, string key)
        {
            var o = obj.SelectToken(key);
            return (o == null) ? "" : (string)o;
        }

        public static bool SelectBoolToken(this JObject obj, string key)
        {
            var o = obj.SelectToken(key);
            return (o == null) ? false : (bool)o;
        }


    }
}
