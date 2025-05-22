using System;
using System.Net;
using System.Text.RegularExpressions;

namespace VSW.Lib.Global
{
    public class Html
    {
        public static string _divContent = @"<div class=""Highlighter"">{0}</div>";

        private static string Get(string html)
        {
            if (string.IsNullOrEmpty(_divContent))
                return string.Empty;

            string pattern = @"<\s*div[^>]*>(.*?)<\s*/div\s*>";
            
            var collection = Regex.Matches(html, pattern);
            foreach (Match match in collection)
            {
                string value = match.Value;
                int marker = value.IndexOf(">");
                string innterHtml = value.Substring(marker + 1, value.Length - (marker + 7));

                if (Regex.Match(innterHtml, pattern).Success)
                    innterHtml = Get(innterHtml);

                string wrappedText = string.Format(_divContent, innterHtml);
                string modifiedValue = value.Replace(innterHtml, wrappedText);

                html = html.Replace(value, modifiedValue);
            }

            return html;
        }

        public static string GetHtml(string url)
        {
            using (var client = new WebClient())
            {
                return Get(client.DownloadString(url));
            }
        }
    }
}