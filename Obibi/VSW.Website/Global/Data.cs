using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using VSW.Core;

namespace VSW.Website.Global
{
    public static class Data
    {
        public static string RemoveAllCRLF(string strinput)
        {
            return Regex.Replace(strinput, "\r|\t|\n", string.Empty);
        }
        public static string RemoveAllTag(string strinput)
        {
            return Regex.Replace(strinput, "<.*?>", string.Empty);
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
