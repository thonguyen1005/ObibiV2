using Newtonsoft.Json;
using System.IO;
using System.Text;

namespace VSW.Lib.Global
{
    public static class DataFormatHelper
    {
        public static string EscapeStringValue(string value)
        {
            string dataEncode = string.Empty;
            var stringBuilder = new StringBuilder();
            using (var jsonWriter = new JsonTextWriter(new StringWriter(stringBuilder)))
            {
                jsonWriter.StringEscapeHandling = StringEscapeHandling.EscapeNonAscii;
                jsonWriter.WriteValue(value);
                jsonWriter.Close();
                dataEncode = stringBuilder.ToString();
            }

            dataEncode = dataEncode.Replace("\"{", "{").Replace("}\"", "}").Replace("\\\"", "\"").Replace("/", "\\/");

            return dataEncode;
        }
    }
}