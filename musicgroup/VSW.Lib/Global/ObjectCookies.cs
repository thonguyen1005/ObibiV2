using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace VSW.Lib.Global
{
    public static class ObjectCookies<T>
    {
        public static bool Exists(string key)
        {
            key = "OBJ_" + key;

            return Cookies.Exists(key);
        }

        public static void SetValue(string key, T value)
        {
            key = "OBJ_" + key;

            try
            {
                var serializer = new DataContractJsonSerializer(typeof(T));
                var memoryStream = new MemoryStream();
                serializer.WriteObject(memoryStream, value);

                var result = Convert.ToBase64String(memoryStream.ToArray());
                memoryStream.Close();

                Cookies.SetValue(key, result, true);
            }
            catch
            {
                Cookies.Remove(key);
            }
        }

        public static T GetValue(string key)
        {
            key = "OBJ_" + key;

            var t = default(T);

            if (!Cookies.Exists(key))
                return t;

            try
            {
                var result = Cookies.GetValue(key, true);
                var arrBytes = Convert.FromBase64String(result);

                var memoryStream = new MemoryStream();
                memoryStream.Write(arrBytes, 0, arrBytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var serializer = new DataContractJsonSerializer(typeof(T));
                t = (T)serializer.ReadObject(memoryStream);

                memoryStream.Close();
            }
            catch
            {
                Cookies.Remove(key);
            }

            return t;
        }

        public static void Remove(string key)
        {
            key = "OBJ_" + key;

            Cookies.Remove(key);
        }
    }
}