using System;
using System.Text;
using System.Text.RegularExpressions;

namespace VSW.Lib.Global
{
    public static class Data
    {
        public static string RemoveAllCrlf(string strinput)
        {
            return Regex.Replace(strinput, "\r|\t|\n", string.Empty);
        }

        public static string RemoveAllTag(string strinput)
        {
            return Regex.Replace(strinput, "<.*?>", string.Empty);
        }

        public static string GetTooltip(string s, int length)
        {
            try
            {
                s = RemoveContent(s, "[if", "[endif]");
                s = RemoveContent(s, "<!--", "-->");

                s = RemoveAllTag(s);
                s = RemoveAllCrlf(s);

                s = CutString(s, length);

                s = s.Replace("'", "\\'");
                s = s.Replace("\"", "");

                return s;
            }
            catch
            {
                return string.Empty;
            }
        }

        public static string CutString(string s, int length)
        {
            if (s.Length <= length) return s;

            if (length > 0)
                s = s.Substring(0, length);

            var index = s.LastIndexOf(" ", StringComparison.Ordinal);

            if (index > 0)
                s = s.Substring(0, index);

            return s + " ...";
        }

        public static string Base64Encode(string s)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(s));
        }

        public static string Base64Decode(string s)
        {
            s = Encoding.UTF8.GetString(Convert.FromBase64String(s));
            return s.Replace("\0", string.Empty);
        }

        public static string GetContent(string sHtml, string sBegin, string sEnd)
        {
            var i = sHtml.IndexOf(sBegin, StringComparison.Ordinal);
            if (i <= -1) return string.Empty;

            var j = sHtml.IndexOf(sEnd, i + sBegin.Length, StringComparison.Ordinal);
            return j > -1 ? sHtml.Substring(i, j - i + sEnd.Length) : string.Empty;
        }

        public static string RemoveContentOne(string sHtml, string sBegin, string sEnd)
        {
            while (true)
            {
                var i = sHtml.IndexOf(sBegin, StringComparison.Ordinal);
                if (i > -1)
                {
                    var j = sHtml.IndexOf(sEnd, i + sBegin.Length, StringComparison.Ordinal);
                    if (j > -1)
                    {
                        var sTemp = sHtml.Substring(i, j - i + sEnd.Length);

                        sHtml = sHtml.Replace(sTemp, string.Empty);
                    }
                }

                break;
            }

            return sHtml;
        }

        public static string RemoveContent(string sHtml, string sBegin, string sEnd)
        {
            return RemoveContent(sHtml, sBegin, sEnd, string.Empty);
        }

        public static string RemoveContent(string sHtml, string sBegin, string sEnd, string sNotIn)
        {
            while (true)
            {
                var i = sHtml.IndexOf(sBegin, StringComparison.Ordinal);
                if (i > -1)
                {
                    var j = sHtml.IndexOf(sEnd, i + sBegin.Length, StringComparison.Ordinal);
                    if (j > -1)
                    {
                        var sTemp = sHtml.Substring(i, j - i + sEnd.Length);

                        if (sNotIn != string.Empty && sTemp.IndexOf(sNotIn, StringComparison.Ordinal) > -1)
                        {
                            return sHtml;
                        }

                        sHtml = sHtml.Replace(sTemp, string.Empty);
                        return RemoveContent(sHtml, sBegin, sEnd, sNotIn);
                    }
                }

                break;
            }

            return sHtml;
        }

        public static string EscapeQuote(string s)
        {
            return EscapeQuoteButNotTrim(s).Trim();
        }

        public static string EscapeQuoteButNotTrim(string s)
        {
            return $"'{EscapeQuoteReg(s)}'";
        }

        public static string Enquote(string s)
        {
            return s.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "\\r").Replace("\t", "\\t").Replace("\b", "\\b").Replace("\f", "\\f");
        }

        public static string EscapeQuoteReg(string s)
        {
            return Regex.Replace(Regex.Replace(s, "'", "''"), "(?:delete|select|drop|create|--)", string.Empty);
        }

        public static string FormatRemoveSql(string s)
        {
            if (s == null) return null;

            var badCommands = ";,--,create,drop,select,insert,delete,update,union,sp_,xp_".Split(',');

            int intCommand;
            for (intCommand = 0; intCommand <= badCommands.Length - 1; intCommand++)
            {
                s = Regex.Replace(s, badCommands[intCommand], " ", RegexOptions.IgnoreCase);
            }

            s = s.Replace("'", "''").Replace("[", string.Empty).Replace("]", string.Empty).Replace("(", string.Empty).Replace(")", string.Empty);

            return s;
        }

        public static string RemoveVietNamese(string s)
        {
            const string findText = "áàảãạâấầẩẫậăắằẳẵặđéèẻẽẹêếềểễệíìỉĩịóòỏõọôốồổỗộơớờởỡợúùủũụưứừửữựýỳỷỹỵÁÀẢÃẠÂẤẦẨẪẬĂẮẰẲẴẶĐÉÈẺẼẸÊẾỀỂỄỆÍÌỈĨỊÓÒỎÕỌÔỐỒỔỖỘƠỚỜỞỠỢÚÙỦŨỤƯỨỪỬỮỰÝỲỶỸỴ";
            const string replText = "aaaaaaaaaaaaaaaaadeeeeeeeeeeeiiiiiooooooooooooooooouuuuuuuuuuuyyyyyAAAAAAAAAAAAAAAAADEEEEEEEEEEEIIIIIOOOOOOOOOOOOOOOOOUUUUUUUUUUUYYYYY";

            int index;
            while ((index = s.IndexOfAny(findText.ToCharArray())) != -1)
            {
                var index2 = findText.IndexOf(s[index]);
                s = s.Replace(s[index], replText[index2]);
            }
            return s;
        }

        private static string RemoveNotAbcChar(string s)
        {
            var result = string.Empty;
            foreach (var t in s)
            {
                var c = t;

                if (c == 160)
                    c = ' ';

                if (char.IsLetterOrDigit(c) || char.IsWhiteSpace(c))
                    result += c.ToString();
                else if (c == '-')
                    result += c.ToString();
            }
            return result;
        }

        public static string GetCode(string s)
        {
            s = RemoveNotAbcChar(RemoveVietNamese(s));

            return s.Trim().Replace(" ", "-")
                .Replace("'", "")
                .Replace("/", "-")
                .Replace("*", "-")
                .Replace("\\", "-")
                .Replace("--", "-")
                .Replace("--", "-").ToLower();
        }
    }
}