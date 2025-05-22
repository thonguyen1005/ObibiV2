using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace VSW.Core
{
    public static class StringExtensions
    {
        /// <summary>
        /// Các ký tự chữ cái, cả hoa và thường, và các chữ số
        /// </summary>
        public static readonly char[] AlphNums = new[]
        {
            '0','2','3','4','5','6','8','9',
            'a','b','c','d','e','f','g','h','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','J','K','L','M','N','P','R','S','T','U','V','W','X','Y','Z'
        };
        /// <summary>
        /// Các ký tự chữ cái, cả hoa và thường
        /// </summary>
        public static readonly char[] Letters = new[]
        {
            'a','b','c','d','e','f','g','h','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z',
            'A','B','C','D','E','F','G','H','J','K','L','M','N','P','R','S','T','U','V','W','X','Y','Z'
        };
        /// <summary>
        /// Các ký tự chữ cái thường
        /// </summary>
        public static readonly char[] LowerLetters = new[]
        {
            'a','b','c','d','e','f','g','h','j','k','m','n','p','q','r','s','t','u','v','w','x','y','z'
        };
        /// <summary>
        /// Các ký tự chữ cái hoa
        /// </summary>
        public static readonly char[] UpperLetters = new[]
        {
            'A','B','C','D','E','F','G','H','J','K','L','M','N','P','R','S','T','U','V','W','X','Y','Z'
        };
        /// <summary>
        /// Các ký tự chữ số
        /// </summary>
        public static readonly char[] Numbers = new[]
        {
            '0','2','3','4','5','6','8','9'
        };

        /// <summary>
        /// Các ký tự đặc biệt, lưu ý cần trùng với các ký tự đặc biệt trong PATTERN_XXX
        /// </summary>
        public static readonly char[] SpecialLetters = new[]
        {
            '!','@','#','$','%','^','&','*','(',')','_'
        };

        public static bool IsEmpty(this string data)
        {
            return string.IsNullOrWhiteSpace(data);
        }

        public static bool IsNotEmpty(this string data)
        {
            return !data.IsEmpty();
        }

        public static string IfEmpty(this string value, string defaultValue)
        {
            return value.IsEmpty() ? defaultValue : value;
        }

        public static string GetEmailDomain(this string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return "";
            }
            var p = email.IndexOf('@');
            if (p <= 0)
            {
                return "";
            }
            var domain = email.Substring(p + 1);
            return domain;
        }

        public static string FormatXml(this string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }

        public static string ToPascalCase(this string value)
        {
            char firstLetter = char.ToUpper(value[0]);
            value = firstLetter.ToString() + (value.Length > 1 ? value.Remove(0, 1).ToLower() : "");
            return value;
        }

        /// <summary>
        /// Ex batch_code --> BatchCode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SpaceToPascalCase(this string value)
        {
            string[] arr = value.Split("_");
            string rs = "";
            int i = 0;
            foreach (var e in arr)
            {
                var a = e.ToPascalCase();
                rs += a;
                i++;
            }

            return rs;
        }


        /// <summary>
        /// Ex batch_code --> batchCode
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string SpaceToCamelCase(this string value)
        {
            string[] arr = value.Split("_");
            string rs = "";
            int i = 0;
            foreach (var e in arr)
            {
                var a = e.ToPascalCase();
                if (i == 0)
                {
                    a = a.ToLower();
                }

                rs += a;

                i++;
            }

            return rs;
        }

        /// <summary>
        /// Ex BatchCode --> batch_code
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Pascal2Lower(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }
            var result = "";
            int len = value.Length;
            for (int i = 0; i < len; i++)
            {
                var c = value[i];
                if (char.IsLower(c))
                {
                    result += c.ToString();
                }
                else
                {
                    var lower = char.ToLower(c).ToString();
                    result += (i == 0) ? lower : "_" + lower;
                }
            }
            return result;
        }

        public static string GetNameOfClass(this string fullName)
        {
            int p = fullName.LastIndexOf(StringConst.PROPERTY_SEPARATE_TOKEN);
            string name = p >= 0 ? fullName.Substring(p + 1) : fullName;
            return name;
        }

        public static string GetNamespaceOfClass(this string fullName)
        {
            int p = fullName.LastIndexOf(StringConst.PROPERTY_SEPARATE_TOKEN);
            string nspace = p >= 1 ? fullName.Substring(0, p) : string.Empty;
            return nspace;
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string if it exceeds maximum length.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        public static string Truncate(this string str, int maxLength)
        {
            if (str == null)
            {
                return null;
            }

            if (str.Length <= maxLength)
            {
                return str;
            }

            return str.Left(maxLength);
        }

        /// <summary>
        /// Gets a substring of a string from end of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Right(this string str, int len)
        {
            if (str.IsEmpty())
            {
                return string.Empty;
            }

            if (str.Length < len)
            {
                return str;
            }

            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// Gets a substring of a string from beginning of the string.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="str"/> is null</exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="len"/> is bigger that string's length</exception>
        public static string Left(this string str, int len)
        {
            if (str == null)
            {
                return string.Empty;
            }

            if (str.Length < len)
            {
                return str;
            }

            return str.Substring(0, len);
        }

        public static string Format(this string value, params object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return value;
            }

            return string.Format(value, args);
        }

        public static bool EqualsIgnoreCase(this string src, string dest)
        {
            if (src == null)
            {
                return dest == null;
            }

            return src.Equals(dest, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string GetFullParentId(this string currentId, string currentFullParentId = null)
        {
            if (currentFullParentId.IsEmpty())
            {
                currentFullParentId = "/";
            }

            return currentFullParentId + currentId + "/";
        }

        public static string[] SplitFullParentId(this string fullParentId)
        {
            return fullParentId.SplitWithTrim("/");
        }

        public static bool NotEqualsIgnoreCase(this string src, string dest)
        {
            return !src.EqualsIgnoreCase(dest);
        }

        public static bool NotEquals(this string src, string dest)
        {
            return !src.Equals(dest);
        }

        /// <summary>
        /// Return Empty if null
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimToEmpty(this string str)
        {
            if (str.IsEmpty())
                return string.Empty;

            return str.Trim();
        }

        public static string NewId()
        {
            return Guid.NewGuid().ToString("N");
        }


        public static string BuildKey(this object[] list)
        {
            return string.Join("@", list);
        }

        public static string BuildKey<T>(T obj, Func<T, object>[] exps)
        {
            var lst = new List<object>();
            foreach (var exp in exps)
            {
                var value = exp(obj);
                lst.Add(value);
            }

            return BuildKey(lst.ToArray());
        }

        /// <summary>
        /// Trim out all in-string White Spaces using String-to-Char[] conversion
        /// https://www.codeproject.com/Articles/1014982/Efficient-Algorithms-to-Trim-In-String-White-Blank
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string TrimWithin(this string str)
        {
            char[] arInput;
            char[] arBuffer;
            string result = string.Empty;
            try
            {
                // basic input validation: cannot be null, empty or white space
                if (str.IsEmpty())
                {
                    return str;
                }

                arInput = str.ToCharArray();
                // create buffer char[] array to populate without white spaces
                arBuffer = new char[arInput.Length];
                // set initial index of buffer array
                int idx = 0;
                // iterate through input array _arInput
                // and populate buffer array excluding white spaces
                for (int i = 0; i < arInput.Length; i++)
                {
                    char ch = arInput[i];
                    if (!char.IsWhiteSpace(ch))
                    {
                        arBuffer[idx] = ch;
                        idx++;
                    }
                }

                return new string(arBuffer, 0, idx);
            }
            catch
            {
                throw;
            }
            finally
            {
                arInput = null;
                arBuffer = null;
            }
        }

        /// <summary>
        /// Reduce in-string White Spaces sequences to a single one
        /// https://www.codeproject.com/Articles/1014982/Efficient-Algorithms-to-Trim-In-String-White-Blank
        /// </summary>
        /// <param name="str">string</param>
        /// <returns>string</returns>
        public static string TrimWithin2One(this string str)
        {
            char[] arInput;
            char[] arBuffer;
            string result = string.Empty;
            try
            {
                // basic input validation: cannot be null, empty or white space
                if (str.IsEmpty())
                {
                    return str;
                }

                str = str.Trim();

                arInput = str.ToCharArray();
                arBuffer = new char[arInput.Length];
                // insert first element from  input array into buffer
                arBuffer[0] = arInput[0];
                // set initial index of buffer array
                int idx = 1;
                // iterate through input array starting from 2nd element
                // and populate buffer array reducing amount of white spaces
                for (int i = 1; i < arInput.Length; i++)
                {
                    char ch = arInput[i];
                    bool isWhiteSpace = char.IsWhiteSpace(ch);
                    if (!isWhiteSpace || (isWhiteSpace && ch != arBuffer[idx - 1]))
                    {
                        arBuffer[idx] = ch;
                        idx++;
                    }
                }

                // convert buffer array into string to return
                return new string(arBuffer, 0, idx);
            }
            catch
            {
                throw;
            }
            finally
            {
                arInput = null;
                arBuffer = null;
            }
        }


        public static string[] Split(this string value, string token = StringConst.DEFAULT_SPLIT_TOKEN,
            StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries)
        {
            if (value == null)
            {
                return null;
            }

            return value.Split(new string[] { token }, option);
        }

        public static string[] SplitWithTrim(this string value, string token = StringConst.DEFAULT_SPLIT_TOKEN,
        StringSplitOptions option = StringSplitOptions.RemoveEmptyEntries)
        {
            if (value == null)
            {
                return null;
            }

            var rs = value.Split(new string[] { token }, option);
            if (rs != null && rs.Length > 0)
            {
                for (int i = 0; i < rs.Length; i++)
                {
                    rs[i] = rs[i].Trim();
                }
            }

            return rs;
        }

        public static string ToBoolString(this bool b)
        {
            return b ? "true" : "false";
        }


        public static bool IsMatch(this string value, string expression)
        {
            return Regex.IsMatch(value, expression, RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Pattern để filter giống File Filter: abc*abc
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatchFilter(this string value, string pattern, string token = StringConst.DEFAULT_SPLIT_TOKEN)
        {
            if (pattern.IsEmpty() || pattern.Equals(token))
            {
                return true;
            }

            value = value.ToLower();
            pattern = pattern.ToLower();

            var keys = pattern.Split(token);
            bool isStartWith = !pattern.StartsWith(token);
            bool isEndWith = !pattern.EndsWith(token);

            int currentIndex = 0;
            for (int i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                bool isOk = false;
                if (i == 0 && isStartWith)
                {
                    isOk = value.StartsWith(key);
                    if (isOk)
                    {
                        currentIndex = key.Length;
                    }
                }
                else if (i == (keys.Length - 1) && isEndWith)
                {
                    isOk = value.EndsWith(key);
                    if (isOk)
                    {
                        currentIndex = value.Length;
                    }
                }
                else
                {
                    var newIndex = value.IndexOf(key, currentIndex);
                    isOk = newIndex >= 0;
                    if (isOk)
                    {
                        currentIndex = newIndex + key.Length;
                    }
                }

                if (!isOk)
                {
                    return false;
                }
            }

            return true;
        }

        #region Cut Sign

        private static Dictionary<String, String> lstToken = new Dictionary<string, string>();
        private static Dictionary<String, String> TokenList
        {
            get
            {
                if (lstToken.Count == 0)
                {
                    lstToken = new Dictionary<string, string>
                                   {
                                       {"a", "á|à|ạ|ả|ã|ă|ắ|ằ|ặ|ẳ|ẵ|â|ấ|ầ|ậ|ẩ|ẫ"},
                                       {"d", "đ"},
                                       {"o", "ó|ò|ọ|ỏ|õ|ơ|ớ|ờ|ợ|ở|ỡ|ô|ố|ồ|ộ|ổ|ỗ"},
                                       {"u", "ú|ù|ụ|ủ|ũ|ư|ứ|ừ|ự|ử|ữ"},
                                       {"e", "é|è|ẹ|ẻ|ẽ|ê|ế|ề|ệ|ể|ễ"},
                                       {"i", "í|ì|ị|ỉ|ĩ"},
                                       {"y", "ý|ỳ|ỵ|ỷ|ỹ"}
                                   };
                }

                return lstToken;
            }
        }

        /// <summary>
        /// Remove Vietnamese Sign
        /// </summary>
        /// <param name="strTemplate"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static string RemoveSign(string strTemplate, bool ignoreCase = true)
        {
            string subTitle = string.Empty;
            if (strTemplate != string.Empty)
            {
                Dictionary<String, String> tokens = TokenList;
                RegexOptions option = ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None;
                foreach (string key in tokens.Keys)
                {
                    strTemplate = Regex.Replace(strTemplate, tokens[key], key, option);
                    if (option == RegexOptions.None)
                    {
                        strTemplate = Regex.Replace(strTemplate, tokens[key].ToUpper(), key.ToUpper(), option);
                    }
                }
                subTitle = strTemplate;
            }
            return subTitle;
        }

        #endregion


        public static string UrlEncode(this string value)
        {
            return WebUtility.UrlEncode(value);
        }

        public static string UrlDecode(this string value)
        {
            return WebUtility.UrlDecode(value);
        }

        public static string HtmlEncode(this string value)
        {
            return WebUtility.HtmlEncode(value);
        }

        public static string HtmlDecode(this string value)
        {
            return WebUtility.HtmlDecode(value);
        }

        public static string removeLeadingZeros(this string str)
        {
            // Regex to remove leading
            // zeros from a string
            string regex = "^0+(?!$)";

            // Replaces the matched
            // value with given string
            str = Regex.Replace(str, regex, "");

            return str;
        }
    }
}
