using System.Web.UI;

namespace VSW.Lib.Global
{
    public static class JavaScript
    {
        public static void Alert(string message, Page page)
        {
            if (message != null)
            {
                Script("AlertScript", $"alert({EscapeQuote(message)})", page);
            }
        }

        public static void Close(Page page)
        {
            Script("CloseScript", "window.close()", page);
        }

        public static void Back(int step, Page page)
        {
            Script("BackScript", "history.go(" + step + ")", page);
        }

        public static void Confirm(string message, string ifTrue, string ifFalse, Page page)
        {
            if (message == null) return;

            var script =
                $"\r\nif (window.confirm({EscapeQuote(message)}))\r\n{{{ifTrue}}}\r\nelse\r\n{{{ifFalse}}}\r\n";
            Script("ConfirmScript", script, page);
        }

        private static string EscapeQuote(string s)
        {
            return Data.EscapeQuote(s);
        }

        public static void Navigate(string href, Page page)
        {
            Script("NavigateScript", "location.href='" + href + "';", page);
        }

        public static void RegisterClientScriptBlock(string key, string script, Page page)
        {
            if (!string.IsNullOrEmpty(script))
            {
                //Script = Script.Replace("''", "|");
                page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, script);
            }
        }

        public static void Script(string key, string script, Page page)
        {
            if (string.IsNullOrEmpty(script)) return;

            var text = $"<script type=\"text/javascript\">{script}</script>";
            page.ClientScript.RegisterStartupScript(page.GetType(), key, text);
        }
    }
}