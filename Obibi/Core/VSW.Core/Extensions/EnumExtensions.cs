using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VSW.Core
{
    public static class EnumExtensions
    {
        public static List<T> GetValues<T>() where T : struct
        {
            var arr = Enum.GetValues(typeof(T));
            var rs = new List<T>();
            foreach (var v in arr)
            {
                rs.Add((T)v);
            }
            return rs;
        }

        public static string GetName(this Enum value)
        {
            return Enum.GetName(value.GetType(), value);
        }

        public static string GetDisplay(this Enum value)
        {
            var att = value.GetAttribute<DisplayAttribute>();
            return att == null ? null : att.Name;
        }

        public static string GetValue(this Enum value)
        {
            var att = value.GetAttribute<ValueAttribute>();
            return att == null ? null : att.Value;
        }

        public static T Parse<T>(string name, T? defaultValue = null) where T : struct
        {
            if (defaultValue.HasValue)
            {
                T v;
                if (TryParse(name, out v))
                {
                    return v;
                }

                return defaultValue.Value;
            }

            return (T)Enum.Parse(typeof(T), name);
        }

        public static bool TryParse<T>(string name, out T v) where T : struct
        {
            return Enum.TryParse(name, out v);
        }

        public static object Parse(Type t, string name)
        {
            return Enum.Parse(t, name);
        }

        public static Dictionary<T, string> GetNames<T>() where T : struct
        {
            var value = GetValues<T>();
            Dictionary<T, string> names = new Dictionary<T, string>();
            foreach (var v in value)
            {
                names.Add((T)v, GetName(v as Enum));
            }

            return names;
        }

        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var memberInfo = type.GetMember(value.ToString());
            var attributes = memberInfo[0].GetCustomAttributes(typeof(T), false);

            return attributes.Length == 0 ? default(T) : (T)attributes[0];
        }

        public static string GetDescription(this Enum value)
        {
            var att = GetAttribute<DescriptionAttribute>(value);
            return att == null ? "" : att.Description;
        }

        public static string GetValueAttr(this Enum value)
        {
            var att = GetAttribute<ValueAttribute>(value);
            return att == null ? "" : att.Value;
        }

        public static Dictionary<T, string> GetDescriptions<T>() where T : struct
        {
            var rs = new Dictionary<T, string>();
            var value = GetValues<T>();
            foreach (var v in value)
            {
                var description = GetDescription(v as Enum);
                rs.Add(v, description);
            }

            return rs;
        }

        public static Dictionary<object, string> GetDescriptions(Type type)
        {
            var rs = new Dictionary<object, string>();
            var value = Enum.GetValues(type);
            foreach (var v in value)
            {
                var description = GetDescription(v as Enum);
                rs.Add(v, description);
            }

            return rs;
        }
        //public static string GetGroupInsurance(this Enum value)
        //{
        //    var type = value.GetType();

        //    string name = Enum.GetName(type, value);
        //    if (name == null) { return null; }

        //    var field = type.GetField(name);
        //    if (field == null) { return null; }

        //    var attr = Attribute.GetCustomAttribute(field, typeof(GroupInsuranceAttribute)) as GroupInsuranceAttribute;
        //    if (attr == null) { return null; }

        //    return attr.GroupInsurance;
        //}
    }
}
