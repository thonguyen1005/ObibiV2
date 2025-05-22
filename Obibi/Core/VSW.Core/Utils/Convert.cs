using System;
using System.Text;

namespace VSW.Core.Utils
{
    public static class Convert
    {
        public static object AutoValue(this object value, Type type)
        {
            bool flag = type == typeof(int);
            object result;
            if (flag)
            {
                result = Convert.ToInt(value);
            }
            else
            {
                bool flag2 = type == typeof(long);
                if (flag2)
                {
                    result = Convert.ToLong(value);
                }
                else
                {
                    bool flag3 = type == typeof(double);
                    if (flag3)
                    {
                        result = Convert.ToDouble(value);
                    }
                    else
                    {
                        bool flag4 = type == typeof(decimal);
                        if (flag4)
                        {
                            result = Convert.ToDecimal(value);
                        }
                        else
                        {
                            bool flag5 = type == typeof(DateTime);
                            if (flag5)
                            {
                                result = Convert.ToDateTime(value);
                            }
                            else
                            {
                                bool flag6 = type == typeof(bool);
                                if (flag6)
                                {
                                    result = Convert.ToBool(value);
                                }
                                else
                                {
                                    result = value;
                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        public static bool ToBool(this object o)
        {
            return Convert.ToBool(o, false);
        }
        public static bool ToBool(this object o, bool defalt)
        {
            bool flag = o == null || o == DBNull.Value || o is DateTime;
            bool result;
            if (flag)
            {
                result = defalt;
            }
            else
            {
                bool flag2 = o is bool;
                if (flag2)
                {
                    result = (bool)o;
                }
                else
                {
                    result = (Convert.ToInt(o) > 0);
                }
            }
            return result;
        }
        public static DateTime ToDateTime(this object o)
        {
            return Convert.ToDateTime(o, DateTime.MinValue);
        }
        public static DateTime ToDateTime(this object o, DateTime defalt)
        {
            bool flag = o != null && o != DBNull.Value;
            DateTime result;
            if (flag)
            {
                bool flag2 = o is DateTime;
                if (flag2)
                {
                    result = (DateTime)o;
                    return result;
                }
                try
                {
                    result = DateTime.Parse(o.ToString());
                    return result;
                }
                catch
                {
                }
            }
            result = defalt;
            return result;
        }
        public static decimal ToDecimal(this object o)
        {
            return Convert.ToDecimal(o, decimal.Zero);
        }
        public static decimal ToDecimal(this object o, decimal defalt)
        {
            bool flag = o != null && o != DBNull.Value && !(o is DateTime);
            decimal result;
            if (flag)
            {
                bool flag2 = o is decimal;
                if (flag2)
                {
                    result = (decimal)o;
                    return result;
                }
                string text = o.ToString().Trim().ToLower();
                string text2 = text;
                string text3 = text2;
                if (text3 != null)
                {
                    if (text3 != null && text3.Length == 0)
                    {
                        result = defalt;
                        return result;
                    }
                    if (text3 == "true")
                    {
                        result = decimal.One;
                        return result;
                    }
                    if (text3 == "false")
                    {
                        result = decimal.Zero;
                        return result;
                    }
                }
                try
                {
                    result = System.Convert.ToDecimal(text);
                    return result;
                }
                catch
                {
                }
            }
            result = defalt;
            return result;
        }
        public static double ToDouble(this object o)
        {
            return Convert.ToDouble(o, 0.0);
        }
        public static double ToDouble(this object o, double defalt)
        {
            bool flag = o != null && o != DBNull.Value && !(o is DateTime);
            double result;
            if (flag)
            {
                bool flag2 = o is double;
                if (flag2)
                {
                    result = (double)o;
                    return result;
                }
                string text = o.ToString().Trim().ToLower();
                string text2 = text;
                string text3 = text2;
                if (text3 != null)
                {
                    if (text3 != null && text3.Length == 0)
                    {
                        result = defalt;
                        return result;
                    }
                    if (text3 == "true")
                    {
                        result = 1.0;
                        return result;
                    }
                    if (text3 == "false")
                    {
                        result = 0.0;
                        return result;
                    }
                }
                try
                {
                    result = System.Convert.ToDouble(text);
                    return result;
                }
                catch
                {
                }
            }
            result = defalt;
            return result;
        }
        public static int ToInt(this object o)
        {
            return Convert.ToInt(o, -1);
        }
        public static int ToInt(this object o, int defalt)
        {
            bool flag = o != null && o != DBNull.Value && !(o is DateTime);
            int result;
            if (flag)
            {
                bool flag2 = o is int;
                if (flag2)
                {
                    result = (int)o;
                    return result;
                }
                string text = o.ToString().Trim().ToLower();
                string text2 = text;
                string text3 = text2;
                if (text3 != null)
                {
                    if (text3 != null && text3.Length == 0)
                    {
                        result = defalt;
                        return result;
                    }
                    if (text3 == "true")
                    {
                        result = 1;
                        return result;
                    }
                    if (text3 == "false")
                    {
                        result = 0;
                        return result;
                    }
                }
                try
                {
                    result = System.Convert.ToInt32(text);
                    return result;
                }
                catch
                {
                }
            }
            result = defalt;
            return result;
        }
        public static long ToLong(this object o)
        {
            return Convert.ToLong(o, -1L);
        }
        public static long ToLong(this object o, long defalt)
        {
            bool flag = o != null && o != DBNull.Value && !(o is DateTime);
            long result;
            if (flag)
            {
                bool flag2 = o is long;
                if (flag2)
                {
                    result = (long)o;
                    return result;
                }
                string text = o.ToString().Trim().ToLower();
                string text2 = text;
                string text3 = text2;
                if (text3 != null)
                {
                    if (text3 != null && text3.Length == 0)
                    {
                        result = defalt;
                        return result;
                    }
                    if (text3 == "true")
                    {
                        result = 1L;
                        return result;
                    }
                    if (text3 == "false")
                    {
                        result = 0L;
                        return result;
                    }
                }
                try
                {
                    result = System.Convert.ToInt64(text);
                    return result;
                }
                catch
                {
                }
            }
            result = defalt;
            return result;
        }
        public static string ToString(this object o)
        {
            return Convert.ToString(o, string.Empty);
        }
        public static string ToString(this object o, string defalt)
        {
            bool flag = o != null && o != DBNull.Value;
            string result;
            if (flag)
            {
                result = o.ToString();
            }
            else
            {
                result = defalt;
            }
            return result;
        }
    }
}
