using System;
using System.IO;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace VSW.Lib.Global
{
    //json la kieu du lieu tra ve khi call 1 ham ajax (thuong tra ve xml hoac json, nhung json nhanh, nhe hon nen thuong dc dung)
    public static class Json
    {
        //JsonModel la 1 object chua toan bo data dc call tu ajax (response). Co the dinh nghia bao nhieu thuoc tinh tuy y (tuy vao nhu cau su dung)
        public class JsonModel
        {
            public string Html { get; set; }
            public string Params { get; set; }
            public string Js { get; set; }
            public string Extension { get; set; }
            public string Extension2 { get; set; }
            public string Extension3 { get; set; }
            public string Extension4 { get; set; }
            public string Extension5 { get; set; }
        }

        public static readonly JsonModel Instance = new JsonModel { Html = string.Empty, Params = string.Empty, Js = string.Empty, Extension = string.Empty };

        public static string JsonSerializer<T>(T t)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memoryStream = new MemoryStream();
            serializer.WriteObject(memoryStream, t);
            var jsonString = Encoding.UTF8.GetString(memoryStream.ToArray());
            memoryStream.Close();

            return jsonString;
        }

        public static T JsonDeserialize<T>(string jsonString)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(jsonString));

            return (T)serializer.ReadObject(memoryStream);
        }

        public static void Create()
        {
            var json = JsonSerializer(Instance);

            System.Web.HttpContext.Current.Response.Clear();
            System.Web.HttpContext.Current.Response.ContentType = "application/json; charset=utf-8";
            System.Web.HttpContext.Current.Response.Write(json);
            System.Web.HttpContext.Current.Response.End();
        }

        public static string GetResponse(string url)
        {
            var uri = new Uri(url);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Method = WebRequestMethods.Http.Get;

            var response = (HttpWebResponse)request.GetResponse();
            var responseStream = response.GetResponseStream();
            if (responseStream == null) return string.Empty;

            var reader = new StreamReader(responseStream);
            var output = reader.ReadToEnd();

            response.Close();

            return output;
        }
    }
}