using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using VSW.Core.Global;
using VSW.Core.Web;
using VSW.Lib.Global.ListItem;
using VSW.Lib.Models;
using VSW.Lib.MVC;
using HttpRequest = VSW.Core.Web.HttpRequest;

namespace VSW.Lib.Global
{
    public class Utils : Core.Web.Utils
    {
        static Regex MobileCheck = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows (ce|phone)|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static Regex TabletCheck = new Regex(@"ipad|android|android 3.0|xoom|sch-i800|playbook|tablet|kindle|nexus", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);
        static Regex MobileVersionCheck = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Compiled);

        public static bool fBrowserIsMobile()
        {
            System.Diagnostics.Debug.Assert(HttpContext.Current != null);
            if (HttpContext.Current.Request != null && HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"] != null)
            {
                var u = HttpContext.Current.Request.ServerVariables["HTTP_USER_AGENT"].ToString();
                if (u.Length < 4)
                    return false;

                if (MobileCheck.IsMatch(u) || TabletCheck.IsMatch(u) || MobileVersionCheck.IsMatch(u.Substring(0, 4)))
                    return true;
            }
            return false;
        }
        public static void GooglePing(string sitemap)
        {
            sitemap = "/ping?sitemap=" + sitemap;

            //GOOGLE
            try
            {
                var request = WebRequest.Create("http://www.google.com/webmasters/tools" + sitemap);
                request.GetResponse();
            }
            catch (Exception ex)
            {
                throw new Exception("Ping sitemap to google had error - " + ex.Message);
            }
        }

        public static string GetDateTimeFromSecond(int value)
        {
            int second = value, hour = 0, minute = 0;
            while (second > 60)
            {
                if (second > 3600)
                {
                    hour = second / 3600;
                    second %= 3600;
                }
                else
                {
                    minute = second / 60;
                    second %= 60;
                }
            }

            return hour + ":" + minute + ":" + second;
        }

        public static string GetHtmlForSeo(string content)
        {
            if (string.IsNullOrEmpty(content))
                return string.Empty;

            content = HttpUtility.HtmlDecode(content);

            var listAutoLinks = ModAutoLinksService.Instance.CreateQuery()
                                    .Where(o => o.Activity == true)
                                    .OrderByDesc(o => o.Order)
                                    .ToList_Cache();

            for (var i = 0; listAutoLinks != null && i < listAutoLinks.Count; i++)
            {
                var value = listAutoLinks[i].Name.Trim();

                if (content.IndexOf(value, StringComparison.OrdinalIgnoreCase) < 0)
                    continue;

                var quantity = listAutoLinks[i].Quantity;
                var replaceValue = @"<a href=""" + listAutoLinks[i].Link + @""" class=""text-primary"" title=""" + listAutoLinks[i].Title + @""" target=""_blank"">" + value + @"</a>";

                var reg = new Regex(value, RegexOptions.IgnoreCase);

                content = quantity > 0 ? reg.Replace(content, replaceValue, quantity) : reg.Replace(content, replaceValue);

                break;
            }
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

                        HtmlNode divContainerNode = HtmlNode.CreateNode("<div class=\"container\">" + listNode[i].OuterHtml + "</div>");

                        listNode[i].ParentNode.ReplaceChild(divContainerNode, listNode[i]);
                    }
                    content = doc.DocumentNode.InnerHtml;
                }
            }
            return content;
        }
        private static HtmlDocument StripHtmlTags(string str)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(str);

            return doc;
        }
        public static string DayOfWeekVn(DateTime dt)
        {
            var arrDayOfWeek = "Chủ nhật,Thứ hai,Thứ ba,Thứ tư,Thứ năm,Thứ sáu,Thứ bảy".Split(',');

            return arrDayOfWeek[(int)dt.DayOfWeek];
        }

        #region charater

        public static string GetFirstLetterOfString(string s)
        {
            if (string.IsNullOrEmpty(s)) return string.Empty;

            while (s.Contains("  "))
            {
                s = s.Replace("  ", " ");
            }

            var result = string.Empty;

            for (var i = 1; i < s.Length; i++)
                if (s[i - 1] == ' ') result += s[i];

            return result;
        }

        public static string GetFirstChar(string title)
        {
            return !string.IsNullOrEmpty(title) ? title[0].ToString() : string.Empty;
        }

        public static string GetCharWithoutFirst(string title)
        {
            return !string.IsNullOrEmpty(title) ? title.Substring(1, title.Length - 1) : string.Empty;
        }

        #endregion charater

        #region validate

        public static string RemoveNonNumeric(string phone)
        {
            var sb = new StringBuilder();
            for (int i = 0; phone != null && i < phone.Length; i++)
                if (char.IsNumber(phone[i]) || phone[i] == '.')
                    sb.Append(phone[i]);

            return sb.ToString();
        }
        public static bool IsPhone(string phone)
        {
            phone = RemoveNonNumeric(phone);

            if (string.IsNullOrEmpty(phone))
                return false;

            if (!phone.StartsWith("0"))
                return false;

            if (phone.Length != 10)
                return false;

            return true;
        }
        public static bool IsEmailAddress(string email)
        {
            return Regex.IsMatch(email.Trim(), @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }

        public static bool IsLoginName(string s)
        {
            if (s.Length < 6 || s.Length > 12) return false;

            if (!char.IsLetter(s[0])) return false;

            return (!Regex.IsMatch(s, "[^a-z0-9_]", RegexOptions.IgnoreCase));
        }

        #endregion validate

        #region data

        public static string ShowDdlMenuByParent(int parentId, int selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(parentId + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                if (parentId < 1)
                    return html;

                var list = WebMenuService.Instance.CreateQuery()
                                              .Where(o => o.ParentID == parentId)
                                              .OrderByAsc(o => o.Order)
                                              .ToList_Cache();

                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].ID == selectId ? "selected" : string.Empty) + " value=\"" + list[i].ID + "\">" + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string ShowDdlMenuByType2(string type, int langId, int selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId + ".2");
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = WebMenuService.Instance.CreateQuery()
                                            .Where(o => o.ParentID == 0 && o.LangID == langId && o.Type == type)
                                            .OrderByAsc(o => o.Order)
                                            .ToList_Cache();

                if (list == null) return html;

                var parentId = list[0].ID;

                list = WebMenuService.Instance.CreateQuery()
                                .Where(o => o.ParentID == parentId)
                                .OrderByAsc(o => o.Order)
                                .ToList_Cache();

                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].ID == selectId ? "selected" : string.Empty) + " value=\"" + list[i].ID + "\">" + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowDdlMenuByTypeVote(string type, int langId, int selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId + ".2");
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = WebMenuService.Instance.CreateQuery()
                                            .Where(o => o.ParentID == 0 && o.LangID == langId && o.Type == type)
                                            .OrderByAsc(o => o.Order)
                                            .ToList_Cache();

                if (list == null) return html;

                var parentId = list[0].ID;

                list = WebMenuService.Instance.CreateQuery()
                                .Where(o => o.ParentID == parentId)
                                .OrderByAsc(o => o.Order)
                                .ToList_Cache();

                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].ID == selectId ? "selected" : string.Empty) + " value=\"" + list[i].ID + "\" data-summary=\"" + Data.RemoveAllCrlf(list[i].Summary) + "\">" + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowDdlMenuByType(string type, int langId, int selectId)
        {
            var html = string.Empty;
            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetList(WebMenuService.Instance, langId, type);
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].Value == selectId.ToString() ? "selected" : string.Empty) + " value=\"" + list[i].Value + "\">&nbsp; " + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowDdlMenuByType5(string type, int langId, string selectlistId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectlistId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetList(WebMenuService.Instance, langId, type);
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    var arrSelect = !string.IsNullOrEmpty(selectlistId) ? selectlistId.Split(',') : null;
                    html += "<option " + (arrSelect != null && System.Array.IndexOf(arrSelect, list[i].Value.ToString()) > -1 ? "selected" : string.Empty) + " value=\"" + list[i].Value + "\">&nbsp; " + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowDdlCodeMenuByType(string type, int langId, string selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListCode(WebMenuService.Instance, langId, type);
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].Value == selectId ? "selected" : string.Empty) + " value=\"" + list[i].Value + "\">&nbsp; " + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowRadioByByType(string type, int langId, int selectId, string name)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetList(WebMenuService.Instance, langId, type);
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += @"<label class=""radioPure radio-inline"">
                              <input type=""radio"" name=""" + name + @""" value=""" + list[i].Value + @""" " + (list[i].Value == selectId.ToString() ? "checked=\"checked\"" : string.Empty) + @" />
                              <span class=""outer""><span class=""inner""></span></span><i>" + list[i].Name + @"</i>
                              </label>";
                    //html += @"<div>
                    //            <input type=""radio"" name=""" + name + @""" value=""" + list[i].Value + @""" " + (list[i].Value == selectId.ToString() ? "checked=\"checked\"" : string.Empty) + @" />
                    //            <span>" + list[i].Name + @"</span>
                    //        </div>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowRadioByByType2(string type, int langId, int selectId, string name)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId + ".2");
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = WebMenuService.Instance.CreateQuery()
                                            .Where(o => o.ParentID == 0 && o.LangID == langId && o.Type == type)
                                            .OrderByAsc(o => o.Order)
                                            .ToList_Cache();

                if (list == null) return html;

                var parentId = list[0].ID;

                list = WebMenuService.Instance.CreateQuery()
                                .Where(o => o.ParentID == parentId)
                                .OrderByAsc(o => o.Order)
                                .ToList_Cache();

                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += @"<label class=""radioPure radio-inline"">
                              <input type=""radio"" name=""" + name + @""" value=""" + list[i].ID + @""" " + (list[i].ID == selectId ? "checked=\"checked\"" : string.Empty) + @" />
                              <span class=""outer""><span class=""inner""></span></span><i>" + list[i].Name + @"</i>
                              </label>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        //3 param:
        //type: loai du lieu - dc dinh nghia trong chuyen muc (News, Product, Showroom,..)
        //langID: ngon ngu dang su dung trong quan tri. Boi vi he thong dc viet da ngon ngu, nen can co tham so xac dinh
        //ngon ngu dang su dung (param LangID tren URL, mac dinh khong co thi la 1 - Tieng Viet)
        //http://localhost:40002/CP/SysMenu/Index.aspx/LangID/2: tieng anh
        //http://localhost:40002/CP/SysMenu/Index.aspx/LangID/3: tieng duc
        //noi chung hau nhu moi thu su dung tren nen tang web deu chi co 2 phuong thuc POST, GET
        //GET: server doc cac gia tri tu URL.
        //POST: day cac gia tri tu form (cac the input) ve server, doc tu server
        //Ca 2 phuong thuc POST, GET deu co the dung class Model de doc.
        public static string ShowDdlShowroomByType(string type, int langId, int selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(type + "." + langId + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetList(WebMenuService.Instance, langId, type);
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].Value == selectId.ToString() ? "selected" : string.Empty) + " value=\"" + list[i].Value + "\">&nbsp; " + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string ShowDdlByConfigkey(string configKey, int selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(configKey + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListByConfigkey(configKey);
                if (configKey == "Mod.ProductState")
                {
                    list = new List<Item>();
                    var listState = ModProductStateService.Instance.CreateQuery().ToList();
                    for (int i = 0; listState != null && i < listState.Count; i++)
                    {
                        list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                    }
                }

                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].Value == selectId.ToString() ? "selected" : string.Empty) + " value=\"" + list[i].Value + "\">" + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string ShowDdlByConfigkey(string configKey, string selectId)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(configKey + "." + selectId);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListByConfigkey(configKey);
                if (configKey == "Mod.ProductState")
                {
                    list = new List<Item>();
                    var listState = ModProductStateService.Instance.CreateQuery().ToList();
                    for (int i = 0; listState != null && i < listState.Count; i++)
                    {
                        list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                    }
                }
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += "<option " + (list[i].Value == selectId ? "selected" : string.Empty) + " value=\"" + list[i].Value + "\">" + list[i].Name + "</option>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string ShowRadioByConfigkey(string configKey, string name, int flag)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(configKey + "." + flag);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListByConfigkey(configKey);
                if (configKey == "Mod.ProductState")
                {
                    list = new List<Item>();
                    var listState = ModProductStateService.Instance.CreateQuery().ToList();
                    for (int i = 0; listState != null && i < listState.Count; i++)
                    {
                        list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                    }
                }
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += @"<label class=""radioPure radio-inline"">
                                <input type=""radio"" name=""" + name + @""" value=""" + list[i].Value + @""" " + ((flag & Core.Global.Convert.ToInt(list[i].Value)) == Core.Global.Convert.ToInt(list[i].Value) ? "checked=\"checked\"" : string.Empty) + @" />
                                <span class=""outer""><span class=""inner""></span></span><i>" + list[i].Name + @"</i>
                            </label>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string ShowCheckBoxByConfigkey(string configKey, string name, int flag)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(configKey + "." + name + "." + flag);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListByConfigkey(configKey);
                if (configKey == "Mod.ProductState")
                {
                    list = new List<Item>();
                    var listState = ModProductStateService.Instance.CreateQuery().ToList();
                    for (int i = 0; listState != null && i < listState.Count; i++)
                    {
                        list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                    }
                }
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    html += @"<label class=""itemCheckBox itemCheckBox-sm"">
                                <input type=""checkbox"" name=""" + name + @""" value=""" + list[i].Value + @""" " + ((flag & Core.Global.Convert.ToInt(list[i].Value)) == Core.Global.Convert.ToInt(list[i].Value) ? "checked=\"checked\"" : string.Empty) + @" />
                                <i class=""check-box""></i>
                                <span>" + list[i].Name + @"</span>
                            </label>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string ShowNameByConfigkey(string configKey, int flag)
        {
            if (flag == 0) return "";
            var html = string.Empty;

            var keyCache = Cache.CreateKey(configKey + "." + flag);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListByConfigkey(configKey);
                if (configKey == "Mod.ProductState")
                {
                    list = new List<Item>();
                    var listState = ModProductStateService.Instance.CreateQuery().ToList();
                    for (int i = 0; listState != null && i < listState.Count; i++)
                    {
                        list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                    }
                }
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    if ((flag & Core.Global.Convert.ToInt(list[i].Value)) != Core.Global.Convert.ToInt(list[i].Value)) continue;

                    html += list[i].Name + "<br />";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string ShowNameByConfigkey2(string configKey, int flag)
        {
            if (flag == 0) return "";
            var html = string.Empty;

            var keyCache = Cache.CreateKey(configKey + "." + flag);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                var list = List.GetListByConfigkey(configKey);
                if (configKey == "Mod.ProductState")
                {
                    list = new List<Item>();
                    var listState = ModProductStateService.Instance.CreateQuery().ToList();
                    for (int i = 0; listState != null && i < listState.Count; i++)
                    {
                        list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                    }
                }
                for (var i = 0; list != null && i < list.Count; i++)
                {
                    if ((flag & Core.Global.Convert.ToInt(list[i].Value)) != Core.Global.Convert.ToInt(list[i].Value)) continue;

                    html += list[i].Name + ",";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }
        public static string GetNameByConfigkey(string configKey, int value)
        {
            var list = List.GetListByConfigkey(configKey);
            if (configKey == "Mod.ProductState")
            {
                list = new List<Item>();
                var listState = ModProductStateService.Instance.CreateQuery().ToList();
                for (int i = 0; listState != null && i < listState.Count; i++)
                {
                    list.Add(new Item(listState[i].Name, listState[i].Value.ToString()));
                }
            }

            var item = list.Find(o => o.Value == value.ToString());

            return item == null ? string.Empty : item.Name;
        }

        public static List<WebMenuEntity> GetListMenuByType(string type, int langId)
        {
            var keyCache = Cache.CreateKey(type + "." + langId);
            var list = Cache.GetValue(keyCache) as List<WebMenuEntity>;
            if (list != null) return list;
            else
            {
                list = WebMenuService.Instance.CreateQuery()
                                            .Where(o => o.ParentID == 0 && o.LangID == langId && o.Type == type)
                                            .OrderByAsc(o => o.Order)
                                            .ToList_Cache();

                if (list == null) return null;

                var parentId = list[0].ID;

                list = WebMenuService.Instance.CreateQuery()
                                .Where(o => o.ParentID == parentId)
                                .OrderByAsc(o => o.Order)
                                .ToList_Cache();

                Cache.SetValue(keyCache, list);
            }

            return list;
        }

        #endregion data

        #region breadcrumb
        public static string GetMapPage(SysPageEntity page)
        {
            var viewPage = Core.Web.Application.CurrentViewPage as ViewPage;

            var html = @"  <a href=""/"" ><i class=""fa fa-home-alt me-1 main-color""></i>" + /*viewPage.Request.Url.Host*/ "" + @"</a>";
            if (page != null)
            {
                if (page.ModuleCode == "MProduct")
                {
                    return html + GetMapPageProduct(viewPage, page);
                    //return html + GetMapPage(viewPage, page);
                }
            }
            return html + GetMapPage(viewPage, page);
        }

        private static string GetMapPage(ViewPage viewPage, SysPageEntity page)
        {
            if (page == null || page.Root) return string.Empty;

            var html = @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span><a href=""" + (viewPage.ViewBag.SecondHand == 1 ? viewPage.GetURL(page.Code + "-secondhand") : viewPage.GetPageURL(page)) + @""">" + page.Name + @"</a>";

            var parent = SysPageService.Instance.GetByID_Cache(page.ParentID);

            if (parent == null || parent.Root || parent.Code == "-") return html;

            return GetMapPage(viewPage, parent) + html;
        }

        private static string GetMapPageProduct(ViewPage viewPage, SysPageEntity page)
        {
            if (page == null || page.Root) return string.Empty;

            var html = @"";
            if (page.ParentID > 0 && (page.ModuleCode != "MProduct" || (page.ModuleCode == "MProduct" && (page.State & 32) != 32)))
            {
                html = @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span><a href=""" + (viewPage.ViewBag.SecondHand == 1 ? viewPage.GetURL(page.Code + "-secondhand") : viewPage.GetPageURL(page)) + @""">" + page.Name + @"</a>";

            }
            var parent = SysPageService.Instance.GetByID_Cache(page.ParentID);

            if (parent == null || parent.Root || parent.Code == "-") return html;

            return GetMapPageProduct(viewPage, parent) + html;
        }

        //private static string GetMapPage(ViewPage viewPage, SysPageEntity page, int index, string urlBrand = "", string nameBrand = "", WebMenuEntity brand = null)
        //{
        //    if (page == null || page.Root) return string.Empty;
        //    var html = "";
        //    if (page.ParentID > 0 && (page.ModuleCode != "MProduct" || (page.ModuleCode == "MProduct" && (page.State & 32) != 32)))
        //    {
        //        string url = "";
        //        if (index == 2)
        //        {
        //            int BrandID = VSW.Core.Global.Convert.ToInt(viewPage.ViewBag.BrandID);
        //            if (BrandID > 0)
        //            {
        //                if (brand == null) brand = WebMenuService.Instance.GetByID_Cache(BrandID);
        //                if (brand != null)
        //                {
        //                    url = page.Code + (!string.IsNullOrEmpty(brand.Code) ? ("-" + brand.Code) : "") + Setting.Sys_PageExt;
        //                    urlBrand = brand.Code;
        //                    nameBrand = brand.Name;
        //                }
        //            }
        //        }
        //        if (!string.IsNullOrEmpty(url) && index == 2)
        //        {
        //            html = @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span><a href=""" + (url) + @""" title=""" + page.Name + " " + nameBrand + @"""><span itemprop=""name"">" + page.Name + " " + nameBrand + @"</a>";
        //        }
        //        else
        //        {
        //            html = @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span><a href=""" + (viewPage.ViewBag.SecondHand == 1 ? viewPage.GetURL(page.Code + "-secondhand") : viewPage.GetPageURL(page)) + @""" title=""" + page.Name + @""">" + page.Name + @"</a>";
        //            if (index == 1)
        //            {
        //                int BrandID = VSW.Core.Global.Convert.ToInt(viewPage.ViewBag.BrandID);
        //                if (BrandID > 0)
        //                {
        //                    if (brand == null) brand = WebMenuService.Instance.GetByID_Cache(BrandID);
        //                    if (brand != null)
        //                    {
        //                        url = page.Code + (!string.IsNullOrEmpty(brand.Code) ? ("-" + brand.Code) : "") + Setting.Sys_PageExt;
        //                        urlBrand = brand.Code;
        //                        nameBrand = brand.Name;
        //                    }
        //                }
        //                if (!string.IsNullOrEmpty(url))
        //                {
        //                    html += @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span><a href=""" + (url) + @""" itemprop=""item"" title=""" + page.Name + " " + nameBrand + @""">" + page.Name + " " + nameBrand + @"</a>";
        //                }
        //            }
        //        }
        //    }
        //    else
        //    {
        //        int BrandID = VSW.Core.Global.Convert.ToInt(viewPage.ViewBag.BrandID);
        //        if (BrandID > 0 && index == 1)
        //        {
        //            html = @"<span class=""mx-2""><i class=""fa fa-chevron-right fz-12""></i></span><a href=""" + (viewPage.ViewBag.SecondHand == 1 ? viewPage.GetURL(viewPage.CurrentPage.Code + "-secondhand") : viewPage.GetPageURL(viewPage.CurrentPage)) + @""" title=""" + viewPage.CurrentPage.Name + @""">" + viewPage.CurrentPage.Name + @"</a>" + html;
        //        }
        //    }
        //    //return html;
        //    var parent = SysPageService.Instance.GetByID_Cache(page.ParentID);
        //    if (parent == null || parent.Root || parent.Code == "-")
        //    {
        //        return html;
        //    }

        //    return GetMapPage(viewPage, parent, index - 1, urlBrand, nameBrand) + html;
        //}

        #endregion breadcrumb

        #region visitor
        public static long Online
        {
            get
            {
                var listOnline = GetOnline();

                if (listOnline == null) return 1;

                return listOnline.Count;
            }
        }

        public static long Visit
        {
            get
            {
                return GetVisit(Range.Total);
            }
        }

        public class Range
        {
            public static string Total = "Total";
            public static string Today = "Today";
            public static string Yesterday = "Yesterday";
            public static string ThisWeek = "ThisWeek";
            public static string LastWeek = "LastWeek";
            public static string ThisMonth = "ThisMonth";
            public static string LastMonth = "LastMonth";
            public static string ThisYear = "ThisYear";
            public static string LastYear = "LastYear";
        }

        public static List<ModOnlineEntity> GetOnline()
        {
            return ModOnlineService.Instance.CreateQuery()
                                    .Where(o => o.TimeValue > DateTime.Now.AddMinutes(-5).Ticks)
                                    .ToList_Cache();
        }

        public static List<ModOnlineEntity> GetOnline(bool isLogin)
        {
            var listOnline = GetOnline();

            if (listOnline == null) return null;

            return listOnline.FindAll(o => o.WebUserID > 0);
        }

        public static long GetVisit(string code)
        {
            return WebSettingService.Instance.CreateQuery()
                            .Select(o => o.Value)
                            .Where(o => o.Code == code)
                            .ToValue_Cache()
                            .ToLong();
        }

        private static WebSettingEntity _oInitRecord;

        private static WebSettingEntity InitRecord(string code)
        {
            if (_oInitRecord == null)
            {
                var record = WebSettingService.Instance.CreateQuery()
                                        .Where(o => o.Code == code)
                                        .ToSingle_Cache();

                if (record == null)
                {
                    record = new WebSettingEntity
                    {
                        ID = 0,
                        Name = code,
                        Code = code,
                        Value = 0
                    };

                    WebSettingService.Instance.Save(record);
                }

                _oInitRecord = record;
            }

            return _oInitRecord;
        }

        private static void Update()
        {
            var dt = DateTime.Now;

            var today = InitRecord(Range.Today);
            var thisWeek = InitRecord(Range.ThisWeek);
            var thisMonth = InitRecord(Range.ThisMonth);
            var thisYear = InitRecord(Range.ThisYear);

            if (dt != dt.Date)
            {
                today.Value++;
                WebSettingService.Instance.Save(today, o => o.Value);

                thisWeek.Value++;
                WebSettingService.Instance.Save(thisWeek, o => o.Value);

                thisMonth.Value++;
                WebSettingService.Instance.Save(thisMonth, o => o.Value);

                thisYear.Value++;
                WebSettingService.Instance.Save(thisYear, o => o.Value);
            }
            else
            {
                var yesterday = InitRecord(Range.Yesterday);

                yesterday.Value = today.Value + 1;
                WebSettingService.Instance.Save(yesterday, o => o.Value);

                today.Value = 1;
                WebSettingService.Instance.Save(today, o => o.Value);

                //ngay cuoi tuan
                if (dt.DayOfWeek == DayOfWeek.Sunday)
                {
                    var lastWeek = InitRecord(Range.LastWeek);

                    lastWeek.Value = thisWeek.Value + 1;
                    WebSettingService.Instance.Save(lastWeek, o => o.Value);

                    thisWeek.Value = 1;
                    WebSettingService.Instance.Save(thisWeek, o => o.Value);
                }

                //ngay cuoi thang
                if (dt.Day == DateTime.DaysInMonth(dt.Year, dt.Month))
                {
                    var lastMonth = InitRecord(Range.LastMonth);

                    lastMonth.Value = thisMonth.Value + 1;
                    WebSettingService.Instance.Save(lastMonth, o => o.Value);

                    thisMonth.Value = 1;
                    WebSettingService.Instance.Save(thisMonth, o => o.Value);

                    //ngay cuoi nam
                    if (dt.Month == 12)
                    {
                        var lastYear = InitRecord(Range.LastYear);

                        lastYear.Value = thisYear.Value + 1;
                        WebSettingService.Instance.Save(lastYear, o => o.Value);

                        thisYear.Value = 1;
                        WebSettingService.Instance.Save(thisYear, o => o.Value);
                    }
                }
            }
        }

        public static void UpdateOnline()
        {
            if (!Config.GetValue("Mod.Visit").ToBool() || Cookies.GetValue("Mod.Online") != null) return;

            Update();

            Cookies.SetValue("Mod.Online", "1", 5, true);

            if (!Config.GetValue("Mod.Online").ToBool()) return;

            ModOnlineService.Instance.Delete(o => o.TimeValue < DateTime.Now.AddMinutes(-5).Ticks);
            ModOnlineService.Instance.Save(new ModOnlineEntity
            {
                SessionID = HttpContext.Current.Session.SessionID,
                TimeValue = DateTime.Now.Ticks,
                IP = HttpRequest.IP
                //WebUserID = WebLogin.IsLogin() ? WebLogin.WebUserID : 0
            });
        }

        #endregion visitor

        #region number to word

        private static string Read(string number)
        {
            var result = "";
            switch (number)
            {
                case "0":
                    result = "không";
                    break;

                case "1":
                    result = "một";
                    break;

                case "2":
                    result = "hai";
                    break;

                case "3":
                    result = "ba";
                    break;

                case "4":
                    result = "bốn";
                    break;

                case "5":
                    result = "năm";
                    break;

                case "6":
                    result = "sáu";
                    break;

                case "7":
                    result = "bảy";
                    break;

                case "8":
                    result = "tám";
                    break;

                case "9":
                    result = "chín";
                    break;
            }
            return result;
        }

        private static string Unit(string number)
        {
            var result = "";

            if (number.Equals("1"))
                result = "";
            if (number.Equals("2"))
                result = "nghìn";
            if (number.Equals("3"))
                result = "triệu";
            if (number.Equals("4"))
                result = "tỷ";
            if (number.Equals("5"))
                result = "nghìn tỷ";
            if (number.Equals("6"))
                result = "triệu tỷ";
            if (number.Equals("7"))
                result = "tỷ tỷ";

            return result;
        }

        private static string Split(string possition)
        {
            var result = "";

            if (possition.Equals("000")) return result;

            if (possition.Length != 3) return result;

            var first = possition.Trim().Substring(0, 1).Trim();
            var middle = possition.Trim().Substring(1, 1).Trim();
            var last = possition.Trim().Substring(2, 1).Trim();

            if (first.Equals("0") && middle.Equals("0"))
                result = " không trăm lẻ " + Read(last.Trim()) + " ";

            if (!first.Equals("0") && middle.Equals("0") && last.Equals("0"))
                result = Read(first.Trim()).Trim() + " trăm ";

            if (!first.Equals("0") && middle.Equals("0") && !last.Equals("0"))
                result = Read(first.Trim()).Trim() + " trăm lẻ " + Read(last.Trim()).Trim() + " ";

            if (first.Equals("0") && Core.Global.Convert.ToInt(middle) > 1 && Core.Global.Convert.ToInt(last) > 0 && !last.Equals("5"))
                result = " không trăm " + Read(middle.Trim()).Trim() + " mươi " + Read(last.Trim()).Trim() + " ";

            if (first.Equals("0") && Core.Global.Convert.ToInt(middle) > 1 && last.Equals("0"))
                result = " không trăm " + Read(middle.Trim()).Trim() + " mươi ";

            if (first.Equals("0") && Core.Global.Convert.ToInt(middle) > 1 && last.Equals("5"))
                result = " không trăm " + Read(middle.Trim()).Trim() + " mươi lăm ";

            if (first.Equals("0") && middle.Equals("1") && Core.Global.Convert.ToInt(last) > 0 && !last.Equals("5"))
                result = " không trăm mười " + Read(last.Trim()).Trim() + " ";

            if (first.Equals("0") && middle.Equals("1") && last.Equals("0"))
                result = " không trăm mười ";

            if (first.Equals("0") && middle.Equals("1") && last.Equals("5"))
                result = " không trăm mười lăm ";

            if (Core.Global.Convert.ToInt(first) > 0 && Core.Global.Convert.ToInt(middle) > 1 && Core.Global.Convert.ToInt(last) > 0 && !last.Equals("5"))
                result = Read(first.Trim()).Trim() + " trăm " + Read(middle.Trim()).Trim() + " mươi " + Read(last.Trim()).Trim() + " ";

            if (Core.Global.Convert.ToInt(first) > 0 && Core.Global.Convert.ToInt(middle) > 1 && last.Equals("0"))
                result = Read(first.Trim()).Trim() + " trăm " + Read(middle.Trim()).Trim() + " mươi ";

            if (Core.Global.Convert.ToInt(first) > 0 && Core.Global.Convert.ToInt(middle) > 1 && last.Equals("5"))
                result = Read(first.Trim()).Trim() + " trăm " + Read(middle.Trim()).Trim() + " mươi lăm ";

            if (Core.Global.Convert.ToInt(first) > 0 && middle.Equals("1") && Core.Global.Convert.ToInt(last) > 0 && !last.Equals("5"))
                result = Read(first.Trim()).Trim() + " trăm mười " + Read(last.Trim()).Trim() + " ";

            if (Core.Global.Convert.ToInt(first) > 0 && middle.Equals("1") && last.Equals("0"))
                result = Read(first.Trim()).Trim() + " trăm mười ";

            if (Core.Global.Convert.ToInt(first) > 0 && middle.Equals("1") && last.Equals("5"))
                result = Read(first.Trim()).Trim() + " trăm mười lăm ";

            return result;
        }

        public static string NumberToWord(string number)
        {
            if (string.IsNullOrEmpty(number)) return "Không đồng.";

            string result = string.Empty, firstPart = string.Empty, lastPart = string.Empty;

            var quotient = Core.Global.Convert.ToInt(number.Length / 3);
            var remainder = number.Length % 3;

            switch (remainder)
            {
                case 0:
                    firstPart = "000";
                    break;

                case 1:
                    firstPart = "00" + number.Trim().Substring(0, 1);
                    break;

                case 2:
                    firstPart = "0" + number.Trim().Substring(0, 2);
                    break;
            }

            if (number.Length > 2)
                lastPart = Core.Global.Convert.ToString(number.Trim().Substring(remainder, number.Length - remainder)).Trim();

            var im = quotient + 1;
            if (remainder > 0)
                result = Split(firstPart).Trim() + " " + Unit(im.ToString().Trim());

            var i = quotient;
            var j = quotient;
            var k = 1;

            while (i > 0)
            {
                var possition = lastPart.Trim().Substring(0, 3).Trim();
                result = result.Trim() + " " + Split(possition.Trim()).Trim();
                quotient = j + 1 - k;

                if (!possition.Equals("000"))
                    result = result.Trim() + " " + Unit(quotient.ToString().Trim()).Trim();

                lastPart = lastPart.Trim().Substring(3, lastPart.Trim().Length - 3);

                i--;
                k++;
            }

            if (result.Trim().Length <= 0) return result.Trim();

            if (result.Trim().Substring(0, 1).Equals("k"))
                result = result.Trim().Substring(10, result.Trim().Length - 10).Trim();

            if (result.Trim().Substring(0, 1).Equals("l"))
                result = result.Trim().Substring(2, result.Trim().Length - 2).Trim();

            result = result.Trim().Substring(0, 1).Trim().ToUpper() +
                     result.Trim().Substring(1, result.Trim().Length - 1).Trim() + " đồng.";

            return result.Trim();
        }

        public static string NumberToWordV2(string number)
        {
            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;

                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;

                            case '5':
                                if (i + j == len - 1)
                                    doc += "lăm ";
                                else
                                    doc += cs[5] + " ";
                                break;

                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += dv[n - j - 1] + " ";
                        }
                    }
                }

                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return doc;
        }

        public static string NumberToWordV3(string input)
        {
            var result = "";

            var number = Core.Global.Convert.ToLong(input);
            if (number < 1) return "0 đồng";

            var billion = number / (long)Math.Pow(10, 9);
            var million = (number - billion * (long)Math.Pow(10, 9)) / (long)Math.Pow(10, 6);
            var thousand = (number - billion * (long)Math.Pow(10, 9) - million * (long)Math.Pow(10, 6)) / (long)Math.Pow(10, 3);

            if (billion > 0) result += billion + " tỷ ";
            if (million > 0) result += million + " triệu ";
            if (thousand > 0) result += thousand + " nghìn ";

            return result.Trim();
        }

        #endregion number to word

        #region media

        public static string GetCodeAdv(ModAdvEntity adv)
        {
            string html;
            var keyCache = Cache.CreateKey(/*"Mod_Adv",*/ "GetCodeAdv." + adv.ID);
            var obj = Cache.GetValue(keyCache);

            if (obj != null) html = obj.ToString();
            else
            {
                html = !string.IsNullOrEmpty(adv.AdvCode) ? adv.AdvCode : string.Empty;

                if (!string.IsNullOrEmpty(adv.File))
                {
                    if (!string.IsNullOrEmpty(adv.URL) && adv.File.EndsWith(".swf", StringComparison.OrdinalIgnoreCase))
                        return GetMedia(adv.File, adv.Width, adv.Height, string.Empty, adv.Name, string.Empty, false);

                    if (string.IsNullOrEmpty(adv.URL)) adv.URL = "/";

                    html = "<a href=\"" + adv.URL + "\" target=\"" + adv.Target + "\" rel=\"nofollow\">" + GetMedia(adv.File, adv.Width, adv.Height, string.Empty, adv.Name, string.Empty, true) + "</a>";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string GetWebPFile(string file, int type, int width, int height)
        {
            if (string.IsNullOrEmpty(file))
                return "data:image/png;base64,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";

            if (file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return file;

            var applicationPath = HttpRequest.ApplicationPath;

            file = HttpUtility.UrlDecode(file);
            var filePath = HttpContext.Current.Server.MapPath(file);

            var target = Path.GetDirectoryName(file.ToLower().Replace("~/data/upload/", "").Replace("/data/upload/", ""));
            Directory.Create(HttpContext.Current.Server.MapPath("~/Data/ResizeImage/" + target));

            var extension = ".webp";
            target = "~/Data/ResizeImage/" + target + "/" + File.FormatFileName(Path.GetFileNameWithoutExtension(file)) + "x" + width + "x" + height + "x" + type + extension;
            var targetPạth = HttpContext.Current.Server.MapPath(target);

            if (System.IO.File.Exists(targetPạth))
                return applicationPath + target.Replace("~/", "/").Replace("\\", "/").Replace("//", "/").Replace("//", "/");

            if (!System.IO.File.Exists(filePath))
                return "data:image/png;base64,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";

            try
            {
                Image.ResizeImageFile(type, filePath, targetPạth, ".webp", width, height, null);
            }
            catch
            {
                // ignored
            }

            if (System.IO.File.Exists(targetPạth))
                return applicationPath + target.Replace("~/", "/").Replace("\\", "/").Replace("//", "/").Replace("//", "/");

            return applicationPath + target.Replace("~/", "/").Replace("\\", "/").Replace("//", "/").Replace("//", "/");
        }
        public static string GetResizeFile(string file, int type, int width, int height)
        {
            if (string.IsNullOrEmpty(file))
                return "/Content/images/noimage.png";

            if (file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return file;

            var applicationPath = HttpRequest.ApplicationPath;

            file = HttpUtility.UrlDecode(file);
            string filePath = "";
            string target = "";
            string extension = "";
            string targetPạth = "";

            try
            {
                filePath = HttpContext.Current.Server.MapPath(file);

                target = Path.GetDirectoryName(file.ToLower().Replace("~/data/upload/", "").Replace("/data/upload/", ""));
                Directory.Create(HttpContext.Current.Server.MapPath("~/Data/ResizeImage/" + target));

                extension = HttpRequest.Browser.ToLower() == "chrome" ? ".webp" : Path.GetExtension(file);
                target = "~/Data/ResizeImage/" + target + "/" + File.FormatFileName(Path.GetFileNameWithoutExtension(file)) + "x" + width + "x" + height + "x" + type + extension;
                targetPạth = HttpContext.Current.Server.MapPath(target);

                if (System.IO.File.Exists(targetPạth))
                    return applicationPath + target.Replace("~/", "/").Replace("\\", "/").Replace("//", "/").Replace("//", "/");

                if (!System.IO.File.Exists(filePath))
                    return "/Content/images/noimage.png";

                Image.ResizeImageFile(type, filePath, targetPạth, extension, width, height, null);
            }
            catch
            {
                // ignored
            }

            if (System.IO.File.Exists(targetPạth))
                return applicationPath + target.Replace("~/", "/").Replace("\\", "/").Replace("//", "/").Replace("//", "/");

            return applicationPath + target.Replace("~/", "/").Replace("\\", "/").Replace("//", "/").Replace("//", "/");
        }

        public static string GetResizeFile(string file, int type, int width, int height, string cssClass, string alt)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(/*"Mod_Adv",*/ "GetResizeFile." + file + "." + type + "." + width + "." + height + "." + cssClass + "." + alt);
            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                if (!string.IsNullOrEmpty(cssClass))
                    html += " class=\"" + cssClass + "\" ";

                if (type != 4 && type != 5)
                {
                    if (width > 0)
                        html += " width=\"" + width + "\" ";
                    else
                        html += " width=\"100%\" ";

                    if (height > 0)
                        html += " height=\"" + height + "\" ";
                    else
                        html += " height=\"auto\" ";
                }

                if (!string.IsNullOrEmpty(alt))
                    html += " alt=\"" + alt + "\" ";

                html = @"<img src=""" + GetResizeFile(file, type, width, height) + @""" " + html + @" />";

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string GetResizeFile(string file, int type, int width, int height, string alt)
        {
            return GetResizeFile(file, type, width, height, string.Empty, alt);
        }

        public static string GetCropFile(string file, int width, int height)
        {
            return GetResizeFile(file, 5, width, height);
        }

        public static string GetMedia(int typeResize, string file, int width, int height, string cssClass, string alt, string addInTag, bool compression)
        {
            var html = string.Empty;

            var keyCache = Cache.CreateKey(/*"Mod_Adv",*/ "GetMedia." + typeResize + "." + file + "." + width + "." + height + "." + cssClass + "." + alt + "." + addInTag + "." + compression);
            //var keyCache = Cache.CreateKey("Mod_Adv");

            var obj = Cache.GetValue(keyCache);
            if (obj != null) html = obj.ToString();
            else
            {
                if (string.IsNullOrEmpty(file))
                    return string.Empty;

                var extension = Path.GetExtension(file).ToLower();
                if (extension == ".swf")
                {
                    file = file.Replace("~/", "/");

                    if (!file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        file = HttpRequest.ApplicationPath + HttpContext.Current.Server.UrlPathEncode(file);

                    html = @"   <object width=""" + (width > 0 ? width.ToString() : "100%") + @""" height=""" + (height > 0 ? height.ToString() : "100%") + @" border=""0"" codebase=""http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0"" classid=""clsid:d27cdb6e-ae6d-11cf-96b8-444553540000"">
                                    <param value=""" + file + @""" name=""movie"">
                                    <param value=""always"" name=""AllowScriptAccess"">
                                    <param value=""high"" name=""quality"">
                                    <param name=""scale"" value=""exactfit"">
                                    <param value=""transparent"" name=""wmode"">
                                    <embed width=""" + (width > 0 ? width.ToString() : "100%") + @""" height=""" + (height > 0 ? height.ToString() : "100%") + @" type=""application/x-shockwave-flash"" scale=""exactfit"" pluginspage=""http://www.macromedia.com/go/getflashplayer"" allowscriptaccess=""always"" wmode=""transparent"" quality=""high"" src=""" + file + @""">
                                </object>";
                }
                else
                {
                    if (!string.IsNullOrEmpty(cssClass))
                        html += " class=\"" + cssClass + "\" ";

                    if (!string.IsNullOrEmpty(alt))
                        html += " alt=\"" + alt + "\" ";

                    if (!compression)
                    {
                        if (!file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            file = HttpRequest.ApplicationPath + HttpContext.Current.Server.UrlPathEncode(file);

                        if (width > 0)
                            html += " width=\"" + width + "\" ";
                        else
                            html += " width=\"100%\" ";

                        if (height > 0)
                            html += " height=\"" + height + "\" ";
                        else
                            html += " height=\"auto\" ";
                    }
                    else
                    {
                        file = GetResizeFile(file, typeResize, width, height);

                        if (typeResize != 4)
                        {
                            if (width > 0)
                                html += " width=\"" + width + "\" ";
                            else
                                html += " width=\"100%\" ";

                            if (height > 0)
                                html += " height=\"" + height + "\" ";
                            else
                                html += " height=\"auto\" ";
                        }
                    }

                    if (!file.StartsWith("/") && !file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        file = "/" + file;

                    html = @"<img src=""" + file + @""" " + html + addInTag + @" />";
                }

                Cache.SetValue(keyCache, html);
            }

            return html;
        }

        public static string GetMedia(string file, int width, int height, string cssClass, string alt, string addInTag, bool compression)
        {
            return GetMedia(2, file, width, height, cssClass, alt, addInTag, compression);
        }

        public static string GetMedia(string file, int width, int height, string cssClass, string alt, string addInTag)
        {
            return GetMedia(2, file, width, height, cssClass, alt, addInTag, true);
        }

        public static string GetMedia(string file, int width, int height, string addInTag)
        {
            return GetMedia(2, file, width, height, string.Empty, string.Empty, addInTag, true);
        }

        public static string GetMedia(string file, int width, int height)
        {
            return GetMedia(2, file, width, height, string.Empty, string.Empty, string.Empty, true);
        }

        public static string GetMedia(string file, int width)
        {
            return GetMedia(2, file, width, 0, string.Empty, string.Empty, string.Empty, true);
        }

        public static string GetMedia(string file)
        {
            return GetMedia(2, file, 0, 0, string.Empty, string.Empty, string.Empty, true);
        }

        public static string GetUrlFile(string file)
        {
            if (string.IsNullOrEmpty(file))
                return "/Content/images/noimage.png";

            if (file.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return file;

            return file.Replace("~/", "/");
        }

        #endregion media
        public static string FormatFloat(decimal money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.##}", money);
        }
        //hàm covert kiểu int thành kiểu float 123,456,789.00
        public static string FormatFloat(int money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.##}", money);
        }
        //hàm covert kiểu int thành kiểu float 123,456,789.00
        public static string FormatFloat(long money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.##}", money);
        }
        //hàm covert kiểu double thành kiểu float 123,456,789.00
        public static string FormatFloat(double money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0.##}", money);
        }
        //hàm covert kiểu decimal thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(decimal money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", money);
        }
        //hàm covert kiểu decimal thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(long money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", money);
        }
        //hàm covert kiểu int thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(int money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", money);
        }
        //hàm covert kiểu double thành kiểu tiền tệ 123,456,789
        public static string FormatMoney(double money)
        {
            return string.Format(CultureInfo.InvariantCulture, "{0:#,##0}", money);
        }
        //hàm covert kiểu DateTime thành kiểu hiển thị ngày dd-MM-yyyy
        public static string FormatDate(DateTime datetime)
        {
            if (datetime == DateTime.MinValue)
            {
                return "";
            }
            return string.Format("{0:dd/MM/yyyy}", datetime);
        }
        public static List<HttpPostedFile> GetFile(HttpFileCollection files, string name)
        {
            var listItem = new List<HttpPostedFile>();
            for (int i = 0; i < files.AllKeys.Length; i++)
            {
                if (files[i].ContentLength < 1 || string.IsNullOrEmpty(files[i].FileName))
                    continue;

                if (files.AllKeys[i] == name)
                    listItem.Add(files[i]);
            }

            return listItem;
        }
        public static Dictionary<string, object> GetLngLat(string address)
        {
            Dictionary<string, object> data = new Dictionary<string, object>();
            try
            {
                string url = "https://maps.googleapis.com/maps/api/geocode/json?address=" + address + "&key=" + Setting.keymap;
                System.Net.WebRequest request = System.Net.WebRequest.Create(url);
                using (System.Net.WebResponse response = (System.Net.HttpWebResponse)request.GetResponse())
                {
                    using (System.IO.StreamReader reader = new System.IO.StreamReader(response.GetResponseStream(), System.Text.Encoding.UTF8))
                    {
                        var Obj = JObject.Parse(reader.ReadToEnd());
                        if (Obj["status"].ToString() == "OK")
                        {
                            string lat = Obj["results"][0]["geometry"]["location"]["lat"].ToString();
                            string lng = Obj["results"][0]["geometry"]["location"]["lng"].ToString();
                            data.Add("lat", lat);
                            data.Add("lng", lng);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                data = new Dictionary<string, object>();
            }
            return data;
        }

        public static int RandomBuyProduct()
        {
            var r = new System.Random();
            return r.Next(200, 999);
        }
    }
}