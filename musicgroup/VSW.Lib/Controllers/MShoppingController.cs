using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO: Shopping", Code = "MShopping", Order = 50)]
    public class MShoppingController : Controller
    {
        public void ActionIndex()
        {
            try
            {
                var lst = ModProductService.Instance.CreateQuery().Where(o => o.Activity == true && (o.State & 16) == 16).ToList();

                var channel = new Channel
                {
                    Title = "Bepvuson Store",
                    Link = "https://bepvuson.vn/",
                    Description = "Bepvuson Store Product Feed",
                    Items = new List<ItemShopping>()
                    
                };
                for (int i = 0; lst != null && i < lst.Count; i++)
                {
                    if (string.IsNullOrEmpty(lst[i].File)) continue;
                    var item = lst[i];
                    var menu = lst[i].GetMenu();
                    channel.Items.Add(new ItemShopping
                    {
                        Id = item.ID.ToString(),
                        Title = string.IsNullOrEmpty(item.PageTitle) ? item.Name : item.PageTitle,
                        Description = string.IsNullOrEmpty(item.PageDescription) ? (string.IsNullOrEmpty(item.Summary) ? item.Name : item.Summary) : item.PageDescription,
                        Link = Core.Web.HttpRequest.Domain + "/" + item.Code + Core.Web.Setting.Sys_PageExt,
                        ImageLink = Core.Web.HttpRequest.Domain + item.File.Replace("~/", "/"),
                        Price = "VND " + item.Price.ToString(),
                        TroductType = (menu != null ? menu.Name : ""),
                        Condition = "new",
                        Availability = "in_stock",
                        IdentifierExists = "no",
                    });                    
                }

                var rss = new Rss
                {
                    Channel = channel
                };

                var serializer = new XmlSerializer(typeof(Rss));

                var settings = new XmlWriterSettings
                {
                    Encoding = new UTF8Encoding(false),
                    Indent = true,
                    OmitXmlDeclaration = false
                };

                using (var stringWriter = new Utf8StringWriter())
                {
                    using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
                    {
                        serializer.Serialize(xmlWriter, rss);
                        string xml = stringWriter.ToString();
                        xml = xml.Replace("_x003A_", ":");
                        ViewPage.Response.ContentType = "text/xml";
                        ViewPage.Response.Write(xml);
                    }
                }

            }
            catch (Exception ex)
            {
                Error.Write(ex.Message + " - Có lỗi xảy ra khi tạo shopping.");

                ViewPage.Response.Write("Có lỗi xảy ra khi tạo shopping.");
            }

            ViewPage.Response.End();
        }

    }

    [XmlRoot("rss")]
    public class Rss
    {
        [XmlAttribute("version")]
        public string Version { get; set; } = "2.0";

        [XmlElement("channel")]
        public Channel Channel { get; set; }
        [XmlNamespaceDeclarations]
        public XmlSerializerNamespaces Xmlns = new XmlSerializerNamespaces(new[] { new XmlQualifiedName("g", "http://base.google.com/ns/1.0") });
    }

    public class Channel
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("item")]
        public List<ItemShopping> Items { get; set; }
    }

    public class ItemShopping
    {
        [XmlElement("g:id")]
        public string Id { get; set; }

        [XmlElement("g:title")]
        public string Title { get; set; }

        [XmlElement("g:description")]
        public string Description { get; set; }

        [XmlElement("g:link")]
        public string Link { get; set; }

        [XmlElement("g:image_link")]
        public string ImageLink { get; set; }

        [XmlElement("g:price")]
        public string Price { get; set; }

        [XmlElement("g:product_type")]
        public string TroductType { get; set; }

        [XmlElement("g:condition")]
        public string Condition { get; set; }

        [XmlElement("g:availability")]
        public string Availability { get; set; }

        [XmlElement("g:identifier_exists")]
        public string IdentifierExists { get; set; }
    }
    public class Utf8StringWriter : StringWriter
    {
        public override Encoding Encoding => Encoding.UTF8;
    }
}