using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using VSW.Core;
using VSW.Core.Utils;
using VSW.Website.Models;

namespace VSW.Website.Global
{
    public static class Utils
    {
        public static string GetFormatString(int decimalDigit = 2)
        {
            var d = decimalDigit == 0 ? "" : ("." + "#".PadLeft(decimalDigit, '#'));
            return "{0:#,##0" + d + "}";
        }

        //hàm covert kiểu decimal thành kiểu float 123,456,789.00
        public static string FormatFloat(decimal money, int decimalDigit = 2)
        {
            string format = GetFormatString(decimalDigit);
            return string.Format(CultureInfo.InvariantCulture, format, money);
        }
        //hàm covert kiểu int thành kiểu float 123,456,789.00
        public static string FormatFloat(int money, int decimalDigit = 0)
        {
            string format = GetFormatString(decimalDigit);
            return string.Format(CultureInfo.InvariantCulture, format, money);
        }
        //hàm covert kiểu int thành kiểu float 123,456,789.00
        public static string FormatFloat(long money, int decimalDigit = 0)
        {
            string format = GetFormatString(decimalDigit);
            return string.Format(CultureInfo.InvariantCulture, format, money);
        }
        //hàm covert kiểu double thành kiểu float 123,456,789.00
        public static string FormatFloat(double money, int decimalDigit = 2)
        {
            string format = GetFormatString(decimalDigit);
            return string.Format(CultureInfo.InvariantCulture, format, money);
        }
        //hàm covert kiểu decimal thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(decimal money, int decimalDigit = 2, string currencySymbol = "")
        {
            string format = GetFormatString(decimalDigit);
            var s = string.Format(CultureInfo.InvariantCulture, format, money);
            if (currencySymbol.IsNotEmpty())
            {
                s += " " + currencySymbol.ToUpper();
            }

            return s;
        }
        //hàm covert kiểu int thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(int money, int decimalDigit = 0, string currencySymbol = "")
        {
            string format = GetFormatString(decimalDigit);
            var s = string.Format(CultureInfo.InvariantCulture, format, money);
            if (currencySymbol.IsNotEmpty())
            {
                s += " " + currencySymbol.ToUpper();
            }

            return s;
        }
        //hàm covert kiểu int thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(long money, int decimalDigit = 0, string currencySymbol = "")
        {
            string format = GetFormatString(decimalDigit);
            var s = string.Format(CultureInfo.InvariantCulture, format, money);
            if (currencySymbol.IsNotEmpty())
            {
                s += " " + currencySymbol.ToUpper();
            }

            return s;
        }
        //hàm covert kiểu double thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(double money, int decimalDigit = 2, string currencySymbol = "")
        {
            string format = GetFormatString(decimalDigit);
            var s = string.Format(CultureInfo.InvariantCulture, format, money);
            if (currencySymbol.IsNotEmpty())
            {
                s += " " + currencySymbol.ToUpper();
            }

            return s;
        }
        //hàm covert kiểu DateTime thành kiểu hiển thị ngày dd/MM/yyyy
        public static string FormatDate(DateTime datetime)
        {
            if (datetime == DateTimeHelper.MIN)
            {
                return "";
            }
            if (datetime.Year <= 1900) return "";
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }
        //hàm covert kiểu DateTime? thành kiểu hiển thị ngày dd/MM/yyyy
        public static string FormatDate(DateTime? datetime)
        {
            if (datetime == DateTimeHelper.MIN)
            {
                return "";
            }
            if (datetime?.Year <= 1900) return "";
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }
        //hàm covert kiểu string thành kiểu hiển thị ngày dd/MM/yyyy
        public static string FormatDate(string sdatetime)
        {
            if (string.IsNullOrEmpty(sdatetime)) return "";
            try
            {
                DateTime dt = sdatetime.ToDateTime();
                return FormatDate(dt);
            }
            catch
            {
                sdatetime = "";
            }
            return sdatetime;
        }
        //hàm covert kiểu DateTime thành kiểu hiển thị ngày dd/MM/yyyy hh:mm:ss
        public static string FormatDateTime(DateTime datetime, string format = null)
        {
            if (datetime.Year <= 1900)
                return "";
            if (format.IsEmpty())
            {
                format = "dd/MM/yyyy " + DateTimeHelper.HHmmss;
            }

            return string.Format("{0:" + format + "}", datetime);
        }
        //hàm covert kiểu string thành kiểu hiển thị ngày dd-MM-yyyy dd-MM-yyyy hh:mm:ss
        public static string FormatDateTime(string sdatetime)
        {
            if (string.IsNullOrEmpty(sdatetime)) return "";
            try
            {
                DateTime dt = sdatetime.ToDateTime();
                return FormatDateTime(dt);
            }
            catch
            {
                sdatetime = "";
            }
            return sdatetime;
        }

        public static string GetHtmlForSeo(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            content = HttpUtility.HtmlDecode(content);

            if (content.Contains(@"widget-toc"))
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(content);
                if (doc != null)
                {
                    var listNode = doc.DocumentNode.SelectNodes(@"//div[contains(@class, 'widget-toc')]");
                    for (int i = 0; listNode != null && i < listNode.Count; i++)
                    {
                        //var temp = StripHtmlTags(listNode[i].InnerHtml);
                        var nodeTitle = listNode[i].SelectSingleNode(@"//p[@class=""toc-title""]");
                        if (nodeTitle != null)
                        {
                            HtmlNode newTitleNew = HtmlNode.CreateNode(@"<button class=""btn-toc"" type=""button"" data-tooltip=""tipsy"" data-position=""left"" original-title=""Nội dung trang"">
                                                                        <i class=""fas fa-list-ol me-2""></i><span>Xem nhanh</span>
                                                                    </button>");
                            listNode[i].ReplaceChild(newTitleNew, nodeTitle);
                        }
                        listNode[i].SetAttributeValue("class", listNode[i].GetAttributeValue("class", "").Replace("widget-toc", "ftoc open"));
                        var ulNodes = listNode[i].SelectNodes("//ul");
                        if (ulNodes != null)
                        {
                            foreach (var ulNode in ulNodes)
                            {
                                // Create a new <div> node
                                HtmlNode divNode = HtmlNode.CreateNode("<nav class=\"toc\"></nav>");

                                // Add the <ul> node as a child of the <div> node
                                divNode.AppendChild(ulNode.Clone());  // Clone to avoid removing the <ul> from its original location

                                // Replace the original <ul> with the new <div> node
                                ulNode.ParentNode.ReplaceChild(divNode, ulNode);
                            }
                        }
                        var liNodes = listNode[i].SelectNodes("//li");
                        if (liNodes != null)
                        {
                            foreach (var liNode in liNodes)
                            {
                                liNode.SetAttributeValue("class", "h3");
                            }
                        }

                        HtmlNode divContainerNode = HtmlNode.CreateNode("<div class=\"container mb-4\">" + listNode[i].OuterHtml + "</div>");

                        listNode[i].ParentNode.ReplaceChild(divContainerNode, listNode[i]);
                    }
                    content = doc.DocumentNode.InnerHtml;
                }
            }
            return content;
        }
    }
}

