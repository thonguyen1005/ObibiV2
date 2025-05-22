using System.Web;

namespace VSW.Lib.Global
{
    public static class Template
    {
        public static string GetHtml(string template, params object[] Params)
        {
            var sHtml = File.ReadText(HttpContext.Current.Server.MapPath("~/Views/Design/" + template + ".html"));

            for (var i = 0; i < Params.Length - 1; i = i + 2)
            {
                sHtml = sHtml.Replace("{" + Params[i] + "}", Params[i + 1].ToString());
            }

            //sHtml = sHtml.Replace("{EMPTY}", string.Empty);

            return sHtml;
        }
    }
}