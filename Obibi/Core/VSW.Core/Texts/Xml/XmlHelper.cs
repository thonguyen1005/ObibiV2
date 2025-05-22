using AutoMapper.Configuration.Conventions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Reflection;
using System.Text;
using System.Xml;

namespace VSW.Core
{
    /// <summary>
    /// Các hàm tiện ích xử lý xml
    /// </summary>
    public static class XmlHelper
    {
        private static KeyValuePair<PropertyInfo, XmlMemberAttribute> GetXMLMapping(PropertyInfo property, string parentTag = null)
        {
            var attribute = property.GetCustomAttribute<XmlMemberAttribute>();
            if (attribute != null)
            {
                if (attribute.TagName.IsEmpty())
                {
                    attribute.TagName = property.Name;
                }

                if (attribute.Type == null)
                {
                    attribute.Type = property.PropertyType;
                }

                if (parentTag.IsNotEmpty() && attribute.TagParent.NotEqualsIgnoreCase(parentTag))
                {
                    attribute = null;
                }
            }

            return new KeyValuePair<PropertyInfo, XmlMemberAttribute>(property, attribute);
        }

        public static T Parse<T>(string input, string rootTag, string parentTag = null) where T : class, new()
        {
            input = (input.IsNotEmpty() ? input.Replace("&","&amp;") : input);
            var rs = new T();
            var b = Parse(rs, rootTag, input, parentTag);
            return rs;
        }

        public static List<T> ParseList<T>(string input, string rootTag, string parentTag = null) where T : class, new()
        {
            var rs = new List<T>();

            if (input.IsEmpty())
            {
                return rs;
            }

            var docXML = new XmlDocument();

            try
            {
                docXML.LoadXml(input);
            }
            catch (Exception)
            {
                return rs;
            }

            var lstAttribute = TypeManager.GetProperties(typeof(T)).Select(x => GetXMLMapping(x.Value.Property, parentTag)).Where(x => x.Value != null).ToList();

            var nodes = docXML.GetElementsByTagName(rootTag);
            var count = nodes.Count;
            for (int i = 0; i < count; i++)
            {
                var node = nodes[i] as XmlElement;
                if (node == null)
                {
                    continue;
                }
                var obj = new T();
                AddXMLToObject(docXML, node, obj, lstAttribute);
                rs.Add(obj);
            }

            return rs;
        }

        /// <summary>
        /// Parse từ xml --> object
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <param name="parentTag">Nếu chỉ định parentTag thì chỉ parse value vào các attribute của parenttag chỉ định</param>
        /// <returns></returns>
        public static bool Parse(object obj, string rootTag, string input, string parentTag = null)
        {
            if (input.IsEmpty() || obj == null)
            {
                return false;
            }

            var docXML = new XmlDocument();

            try
            {
                docXML.LoadXml(input);
            }
            catch (Exception)
            {
                return false;
            }

            var lstAttribute = TypeManager.GetProperties(obj.GetType()).Select(x => GetXMLMapping(x.Value.Property, parentTag)).Where(x => x.Value != null).ToList();
            var nodes = docXML.GetElementsByTagName(rootTag);
            if (nodes.Count > 0)
            {
                var node = nodes[0] as XmlElement;
                AddXMLToObject(docXML, node, obj, lstAttribute);
            }

            return true;
        }


        /// <summary>
        /// Convert dữ liệu từ Object --> XML
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="parentTag">Nếu chỉ định parentTag thì chỉ parse value vào các attribute của parenttag chỉ định</param>
        /// <returns></returns>
        public static string ToXML(this object obj, string rootTag, string parentTag = null, bool ignoreDefaultValue = false)
        {
            var docXml = new XmlDocument();
            docXml.LoadXml("<root></root>");

            if (obj != null)
            {
                List<KeyValuePair<PropertyInfo, XmlMemberAttribute>> lstAttribute = null;
                if (obj is IList && (obj as IList).Count > 0)
                {
                    var lst = obj as IList;
                    lstAttribute = TypeManager.GetProperties(lst[0].GetType()).Select(x => GetXMLMapping(x.Value.Property, parentTag)).Where(x => x.Value != null).ToList();

                    foreach (var item in lst)
                    {
                        var root = docXml.CreateElement(rootTag);
                        docXml.DocumentElement.AppendChild(root);
                        AddXMLToDoc(docXml, root, item, lstAttribute, ignoreDefaultValue);
                    }
                }
                else
                {
                    lstAttribute = TypeManager.GetProperties(obj.GetType()).Select(x => GetXMLMapping(x.Value.Property, parentTag)).Where(x => x.Value != null).ToList();

                    var root = docXml.CreateElement(rootTag);
                    docXml.DocumentElement.AppendChild(root);
                    AddXMLToDoc(docXml, root, obj, lstAttribute, ignoreDefaultValue);
                }
            }

            return docXml.InnerXml;
        }


        private static void AddXMLToDoc(XmlDocument doc, XmlElement parent, object obj, List<KeyValuePair<PropertyInfo, XmlMemberAttribute>> lstAttribute, bool ignoreDefaultValue = false)
        {
            foreach (var attMapping in lstAttribute)
            {
                var v = obj.GetPropValue(attMapping.Key.Name);
                if (ignoreDefaultValue && TypeManager.IsDefaultValue(v))
                {
                    continue;
                }

                var node = doc.CreateElement(attMapping.Value.TagName);
                node.InnerText = ValueToText(v, attMapping.Value);
                parent.AppendChild(node);
            }
        }


        private static string ValueToText(object v, XmlMemberAttribute attribute)
        {
            if (v == null)
            {
                return "";
            }

            if (v is DateTime)
            {
                return ((DateTime)v).ToString(attribute.Format.IsNotEmpty() ? attribute.Format : DateTimeHelper.DD_MM_YYYY_VN);
            }

            return v.ToString();
        }

        private static object TextToValue(string v, XmlMemberAttribute attribute, Type propType)
        {
            if (propType == typeof(DateTime))
            {
                if (v.Length == 6)
                {
                    return DateTime.ParseExact(v, attribute.Format.IsNotEmpty() ? "M/yyyy" : DateTimeHelper.DD_MM_YYYY_VN, null);
                }
                if (v.Length == 9)
                {
                    var lst = v.SplitWithTrim("/");
                    if (lst.Length == 3)
                    {
                        if (lst[0].Length == 1)
                        {
                            v = v.Insert(0, "0");
                        }
                        else if (lst[1].Length == 1)
                        {
                            v = v.Insert(3, "0");
                        }
                    }
                }
                return DateTime.ParseExact(v, attribute.Format.IsNotEmpty() ? attribute.Format : DateTimeHelper.DD_MM_YYYY_VN, null);
            }

            return v.To(propType);
        }


        private static void AddXMLToObject(XmlDocument doc, XmlElement parent, object obj, List<KeyValuePair<PropertyInfo, XmlMemberAttribute>> lstAttribute)
        {
            foreach (var attMapping in lstAttribute)
            {
                var xmlNodes = parent != null ? parent.GetElementsByTagName(attMapping.Value.TagName) : doc.GetElementsByTagName(attMapping.Value.TagName);
                if (xmlNodes == null || xmlNodes.Count == 0)
                {
                    continue;
                }

                var nodeValue = xmlNodes[0].InnerText;
                if (nodeValue.IsNotEmpty())
                {
                    var v = TextToValue(nodeValue, attMapping.Value, attMapping.Key.PropertyType);
                    obj.SetPropValue(attMapping.Key.Name, v);
                }
            }
        }

    }
}
