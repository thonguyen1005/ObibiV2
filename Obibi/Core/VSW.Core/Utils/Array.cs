using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace VSW.Core.Utils
{
    public static class Array
    {
        public static bool[] ToBools(this string source)
        {
            return Array.ToBools(source.Split(new char[]
            {
                ','
            }));
        }
        public static bool[] ToBools(this string[] source)
        {
            bool flag = source == null || source.Length == 0;
            bool[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool[] array = new bool[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    array[i] = Convert.ToBool(source[i]);
                }
                result = array;
            }
            return result;
        }
        public static bool[] ToBools(this string source, char split)
        {
            return Array.ToBools(source.Split(new char[]
            {
                split
            }));
        }
        public static bool[] ToBools(this string source, string split)
        {
            return Array.ToBools(Regex.Split(source, split));
        }
        public static DateTime[] ToDateTimes(this string source)
        {
            return Array.ToDateTimes(source.Split(new char[]
            {
                ','
            }));
        }
        public static DateTime[] ToDateTimes(this string[] source)
        {
            bool flag = source == null || source.Length == 0;
            DateTime[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                DateTime[] array = new DateTime[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    array[i] = Convert.ToDateTime(source[i]);
                }
                result = array;
            }
            return result;
        }
        public static DateTime[] ToDateTimes(this string source, char split)
        {
            return Array.ToDateTimes(source.Split(new char[]
            {
                split
            }));
        }
        public static DateTime[] ToDateTimes(this string source, string split)
        {
            return Array.ToDateTimes(Regex.Split(source, split));
        }
        public static decimal[] ToDecimals(this string source)
        {
            return Array.ToDecimals(source.Split(new char[]
            {
                ','
            }));
        }
        public static decimal[] ToDecimals(this string[] source)
        {
            bool flag = source == null || source.Length == 0;
            decimal[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                decimal[] array = new decimal[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    array[i] = Convert.ToDecimal(source[i]);
                }
                result = array;
            }
            return result;
        }
        public static decimal[] ToDecimals(this string source, char split)
        {
            return Array.ToDecimals(source.Split(new char[]
            {
                split
            }));
        }
        public static decimal[] ToDecimals(this string source, string split)
        {
            return Array.ToDecimals(Regex.Split(source, split));
        }
        public static double[] ToDoubles(this string source)
        {
            return Array.ToDoubles(source.Split(new char[]
            {
                ','
            }));
        }
        public static double[] ToDoubles(this string[] source)
        {
            bool flag = source == null || source.Length == 0;
            double[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                double[] array = new double[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    array[i] = Convert.ToDouble(source[i]);
                }
                result = array;
            }
            return result;
        }
        public static double[] ToDoubles(this string source, char split)
        {
            return Array.ToDoubles(source.Split(new char[]
            {
                split
            }));
        }
        public static double[] ToDoubles(this string source, string split)
        {
            return Array.ToDoubles(Regex.Split(source, split));
        }
        public static int[] ToInts(this string source)
        {
            return Array.ToInts(source.Split(new char[]
            {
                ','
            }));
        }
        public static int[] ToInts(this string[] source)
        {
            bool flag = source == null || source.Length == 0;
            int[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                int[] array = new int[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    array[i] = Convert.ToInt(source[i]);
                }
                result = array;
            }
            return result;
        }
        public static int[] ToInts(this string source, char split)
        {
            return Array.ToInts(source.Split(new char[]
            {
                split
            }));
        }
        public static int[] ToInts(this string source, string split)
        {
            return Array.ToInts(Regex.Split(source, split));
        }
        public static long[] ToLongs(this string source)
        {
            return Array.ToLongs(source.Split(new char[]
            {
                ','
            }));
        }
        public static long[] ToLongs(string[] source)
        {
            bool flag = source == null || source.Length == 0;
            long[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                long[] array = new long[source.Length];
                for (int i = 0; i < source.Length; i++)
                {
                    array[i] = Convert.ToLong(source[i]);
                }
                result = array;
            }
            return result;
        }
        public static long[] ToLongs(this string source, char split)
        {
            return Array.ToLongs(source.Split(new char[]
            {
                split
            }));
        }
        public static long[] ToLongs(this string source, string split)
        {
            return Array.ToLongs(Regex.Split(source, split));
        }
        public static string[] ToString(this string source)
        {
            bool flag = string.IsNullOrEmpty(source);
            string[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = source.EndsWith(",");
                if (flag2)
                {
                    source = source.Substring(1, source.Length - 2);
                }
                result = source.Split(new char[]
                {
                    ','
                });
            }
            return result;
        }
        public static string[] ToString(this string source, char split)
        {
            bool flag = string.IsNullOrEmpty(source);
            string[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = source.EndsWith(split.ToString());
                if (flag2)
                {
                    source = source.Substring(1, source.Length - 2);
                }
                result = source.Split(new char[]
                {
                    split
                });
            }
            return result;
        }
        public static string[] ToString(string source, string split)
        {
            bool flag = string.IsNullOrEmpty(source);
            string[] result;
            if (flag)
            {
                result = null;
            }
            else
            {
                bool flag2 = source.EndsWith(split);
                if (flag2)
                {
                    source = source.Substring(1, source.Length - split.Length - 1);
                }
                result = Regex.Split(source, split);
            }
            return result;
        }
        public static string ToString(this string[] source)
        {
            bool flag = source == null || source.Length == 0;
            string result;
            if (flag)
            {
                result = string.Empty;
            }
            else
            {
                string text = string.Empty;
                for (int i = 0; i < source.Length; i++)
                {
                    string text2 = source[i].Trim();
                    bool flag2 = text2 != string.Empty;
                    if (flag2)
                    {
                        bool flag3 = text == string.Empty;
                        if (flag3)
                        {
                            text = text2;
                        }
                        else
                        {
                            text = text + "," + text2;
                        }
                    }
                }
                result = text;
            }
            return result;
        }
        public static string ToString(this int[] source)
        {
            bool flag = source == null || source.Length == 0;
            string result;
            if (flag)
            {
                result = string.Empty;
            }
            else
            {
                string text = string.Empty;
                for (int i = 0; i < source.Length; i++)
                {
                    bool flag2 = i == 0;
                    if (flag2)
                    {
                        text = source[i].ToString();
                    }
                    else
                    {
                        text = text + "," + source[i].ToString();
                    }
                }
                result = text;
            }
            return result;
        }
        public static string ToString(this int[] source, char split)
        {
            bool flag = source == null || source.Length == 0;
            string result;
            if (flag)
            {
                result = string.Empty;
            }
            else
            {
                string text = string.Empty;
                for (int i = 0; i < source.Length; i++)
                {
                    bool flag2 = i == 0;
                    if (flag2)
                    {
                        text = source[i].ToString();
                    }
                    else
                    {
                        text = text + split.ToString() + source[i].ToString();
                    }
                }
                result = text;
            }
            return result;
        }
        public static string ToString(this string[] source, char split)
        {
            bool flag = source == null || source.Length == 0;
            string result;
            if (flag)
            {
                result = string.Empty;
            }
            else
            {
                string text = string.Empty;
                for (int i = 0; i < source.Length; i++)
                {
                    string text2 = source[i];
                    string text3 = text2.Trim();
                    bool flag2 = text3 == string.Empty;
                    if (!flag2)
                    {
                        bool flag3 = text == string.Empty;
                        if (flag3)
                        {
                            text = text3;
                        }
                        else
                        {
                            text = text + split.ToString() + text3;
                        }
                    }
                }
                result = text;
            }
            return result;
        }
        public static List<object> Tolist(this string[] source)
        {
            return new List<object>(source);
        }
    }
}
