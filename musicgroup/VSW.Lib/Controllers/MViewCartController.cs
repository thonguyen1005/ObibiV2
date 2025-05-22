using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;
using VSW.Core.Web;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;
using Utils = VSW.Lib.Global.Utils;

namespace VSW.Lib.Controllers
{
    [ModuleInfo(Name = "MO : Giỏ hàng", Code = "MViewCart", Order = 50)]
    public class MViewCartController : Controller
    {
        public void ActionIndex(ModOrderEntity item, MViewCartModel model)
        {
            var cart = new Cart();
            if (cart.Items.Count < 1) ViewPage.Redirect("Giỏ hàng của bạn chưa có sản phẩm nào.", string.IsNullOrEmpty(model.returnpath) ? "/" : model.returnpath);
            ViewBag.Data = item;
            ViewBag.Model = model;

            //SEO
            ViewPage.CurrentPage.PageURL = ViewPage.CurrentURL;
            ViewPage.CurrentPage.PageFile = Core.Web.HttpRequest.Domain + Utils.GetUrlFile(ViewPage.CurrentPage.File);
        }

        //them gio hang
        public void ActionAdd(ModOrderEntity item, MViewCartModel model)
        {

            var cart = new Cart();
            if (model.SizeID < 0) model.SizeID = 0;
            if (model.ColorID < 0) model.ColorID = 0;
            if (model.Quantity <= 0) model.Quantity = 1;
            var _item = cart.Find(new CartItem() { ProductID = model.ProductID, SizeID = model.SizeID, ColorID = model.ColorID });
            if (_item == null)
            {
                cart.Add(new CartItem
                {
                    ProductID = model.ProductID,
                    SizeID = model.SizeID,
                    ColorID = model.ColorID,
                    Quantity = model.Quantity,
                });
            }
            else
            {
                _item.Quantity += model.Quantity;
            }

            cart.Save();

            ViewBag.Data = item;
            ViewBag.Model = model;

            ViewPage.Response.Redirect("~/" + ViewPage.ViewCartUrl + "?returnpath=" + model.returnpath);
        }

        public void ActionDelete(ModOrderEntity item, MViewCartModel model)
        {
            var cart = new Cart();

            cart.Remove(cart.Items[model.Index]);
            cart.Save();

            ViewBag.Data = item;
            ViewBag.Model = model;

            ViewPage.Response.Redirect("~/" + ViewPage.ViewCartUrl + "?returnpath=" + model.returnpath);
        }

        public void ActionUpdate(ModOrderEntity item, MViewCartModel model)
        {
            var cart = new Cart();

            if (model.Quantity <= 0) model.Quantity = 1;
            cart.Items[model.Index].Quantity = model.Quantity;
            if (model.SizeID > 0) cart.Items[model.Index].SizeID = model.SizeID;
            if (model.ColorID > 0) cart.Items[model.Index].ColorID = model.ColorID;

            cart.Save();

            ViewBag.Data = item;
            ViewBag.Model = model;

            ViewPage.Response.Redirect("~/" + ViewPage.ViewCartUrl + "?returnpath=" + model.returnpath);
        }

        public void ActionAddPOST(ModOrderEntity item, MViewCartModel model)
        {
            if (item.Name.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: Họ và tên.");
            if (item.Phone.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: Số điện thoại.");
            else if (!Global.Utils.IsPhone(item.Phone.Trim()))
                ViewPage.Message.ListMessage.Add("Số điện thoại không đúng.");
            if (!string.IsNullOrEmpty(item.Email))
            {
                if (!Utils.IsEmailAddress(item.Email.Trim()))
                    ViewPage.Message.ListMessage.Add("Nhập: đúng đinh dạng Email.");
            }

            if (item.CityID < 1)
                ViewPage.Message.ListMessage.Add("Chọn: Tỉnh/Thành Phố.");
            if (item.DistrictID < 1)
                ViewPage.Message.ListMessage.Add("Chọn: Quận/Huyện.");
            if (item.WardID < 1)
                ViewPage.Message.ListMessage.Add("Chọn: Phường/Xã.");
            if (item.Address.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: Số nhà, tòa nhà, tên thôn xóm, tên đường...");

            if (item.Invoice && item.CompanyName.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: tên công ty.");
            if (item.Invoice && item.CompanyTax.Trim() == string.Empty)
                ViewPage.Message.ListMessage.Add("Nhập: mã số thuế.");

            item.StatusPay = false;
            ViewBag.Data = item;
            ViewBag.Model = model;
            //hien thi thong bao loi
            if (ViewPage.Message.ListMessage.Count > 0)
            {
                var message = ViewPage.Message.ListMessage.Aggregate(string.Empty, (current, t) => current + ("- " + t + "<br />"));
                ViewPage.Alert(message);
            }
            else
            {
                var cart = new Cart();
                item.IP = HttpRequest.IP;
                item.Created = DateTime.Now;
                item.StatusID = 11977;
                item.OrderNews = true;
                ModOrderService.Instance.Save(item);
                item.Code = "DH" + string.Format("{0:ddMMyyyy}", item.Created) + GetOrder(item.ID);
                //luu chi tiet don hang & send mail
                long total = 0;
                long weight = 0;
                string html_details = "";
                for (var i = 0; i < cart.Count; i++)
                {
                    var product = ModProductService.Instance.GetByID(cart.Items[i].ProductID);
                    if (product == null) continue;
                    long wp = product.Weight;
                    var size = ModProductClassifyDetailPriceService.Instance.GetByProperty(cart.Items[i].ProductID, cart.Items[i].ColorID, cart.Items[i].SizeID);
                    long price = product.PriceView;
                    if (size != null)
                    {
                        price = size.Price;
                    }
                    weight += (wp * cart.Items[i].Quantity);
                    long t = cart.Items[i].Quantity * price;
                    total += t;
                    string other = "";
                    var detail = new ModOrderDetailEntity()
                    {
                        ID = 0,
                        OrderID = item.ID,
                        ProductID = product.ID,
                        SizeID = cart.Items[i].SizeID,
                        ColorID = cart.Items[i].ColorID,
                        Quantity = cart.Items[i].Quantity,
                        Price = price,
                        PriceAll = t,
                        Other = other,
                        Created = item.Created,
                    };
                    ModOrderDetailService.Instance.Save(detail);
                    html_details += @"<tr id=""m_-5500805879502696495row_view"" style=""font-weight:bold"">";
                    html_details += @"<td height=""27"" style=""padding: 5px;"" align=""left"" valign=""bottom""><font face=""Arial"" size=""1"" color=""#1C1C1C"">" + (i + 1) + "</font></td>";
                    html_details += @"<td align=""left"" style=""padding: 5px;"" valign=""bottom""><font face=""Arial"" size=""1"" color=""#1C1C1C"">" + product.Name + "</font></td>";
                    html_details += @"<td align=""left"" style=""padding: 5px;"" valign=""bottom""><font face=""Arial"" size=""1"" color=""#1C1C1C"">" + other + "</font></td>";
                    html_details += @"<td align=""left"" style=""padding: 5px;"" valign=""bottom""><font face=""Arial"" size=""1"" color=""#1C1C1C"">" + Utils.FormatMoney(price) + "</font></td>";
                    html_details += @"<td align=""left"" style=""padding: 5px;"" valign=""bottom""><font face=""Arial"" size=""1"" color=""#1C1C1C"">" + cart.Items[i].Quantity + "</font></td>";
                    html_details += @"<td align=""left"" style=""padding: 5px;"" valign=""bottom""><font face=""Arial"" size=""1"" color=""#1C1C1C"">" + Utils.FormatMoney(t) + "</font></td>";
                    html_details += @"</tr>";
                }
                cart.Save();
                if (!string.IsNullOrEmpty(item.SaleCode))
                {
                    var dbQuery = ModSaleService.Instance.CreateQuery()
                                         .Where(o => o.Activity == true)
                                         .Where(o => o.Code == item.SaleCode)
                                         .Where(o => o.DateStart <= DateTime.Now)
                                         .Where(o => o.DateEnd >= DateTime.Now)
                                         .Where(o => (o.MoneyMin <= total));

                    var sale = dbQuery.ToSingle();
                    if (sale == null)
                    {
                        item.SaleMoney = 0;
                        item.SalePercent = 0;
                        item.SaleCode = "";
                    }
                    else
                    {
                        if (sale.Status)
                        {
                            item.SaleMoney = 0;
                            item.SalePercent = 0;
                            item.SaleCode = "";
                        }
                        else
                        {
                            if (sale.Money > 0)
                            {
                                item.SaleMoney = sale.Money;
                                item.SalePercent = 0;
                            }
                            else
                            {
                                item.SalePercent = sale.Percent;
                                item.SaleMoney = System.Convert.ToInt64(sale.Percent * total / 100);
                            }
                            sale.CountUse++;
                            if (sale.CountUse >= sale.Count)
                            {
                                sale.Status = true;
                            }
                            ModSaleService.Instance.Save(sale, o => new { o.CountUse, o.Status });
                            var saledetail = new ModSaleDetailEntity();
                            saledetail.OrderID = item.ID;
                            saledetail.SaleID = sale.ID;
                            saledetail.Money = item.SaleMoney;
                            saledetail.Published = DateTime.Now;
                            ModSaleDetailService.Instance.Save(saledetail);
                        }
                    }
                }
                else
                {
                    item.SaleMoney = 0;
                    item.SalePercent = 0;
                    item.SaleCode = "";
                }
                item.Total = total;
                item.Fee = 0;
                var w = new ModWebUserEntity();
                {
                    if (!string.IsNullOrEmpty(item.Email))
                    {
                        w = ModWebUserService.Instance.GetByEmail(item.Email);
                        if (w == null)
                        {
                            w = ModWebUserService.Instance.GetByPhone(item.Phone);
                        }
                    }
                    else if (!string.IsNullOrEmpty(item.Phone))
                    {
                        w = ModWebUserService.Instance.GetByPhone(item.Phone);
                    }
                    if (w == null)
                    {
                        w = new ModWebUserEntity();
                        w.UserName = (!string.IsNullOrEmpty(item.Email) ? item.Email : item.Phone);
                        if (string.IsNullOrEmpty(w.Password))
                        {
                            w.Password = Security.Md5("Abc@12345");
                        }
                        w.Name = item.Name;
                        w.Email = item.Email;
                        w.Phone = item.Phone;
                        w.Address = item.Address;
                        w.Type = 2;
                        w.Type2 = 1;
                        w.CityID = item.CityID;
                        w.DistrictID = item.DistrictID;
                        w.WardID = item.WardID;
                        w.CompanyName = item.CompanyName;
                        w.CompanyAddress = item.CompanyAddress;
                        w.CompanyTax = item.CompanyTax;
                        w.IP = VSW.Core.Web.HttpRequest.IP;
                        w.Created = DateTime.Now;
                        w.Activity = true;
                        ModWebUserService.Instance.Save(w);
                    }
                    item.WebUserID = w.ID;
                }
                double valuegiam = 0;
                item.SaleMoneyPoint = 0;
                item.Point = 0;
                item.SaleMoney = item.SaleMoney < 0 ? 0 : item.SaleMoney;
                item.SaleCustomer = item.SaleCustomer < 0 ? 0 : item.SaleCustomer;
                item.SaleMoneyPoint = item.SaleMoneyPoint < 0 ? 0 : item.SaleMoneyPoint;
                item.Fee = item.Fee < 0 ? 0 : item.Fee;
                ModOrderService.Instance.Save(item);

                //gui mail
                #region send mail
                string sHtml = "<center><p style=\"width:100%;\"><span style=\"color: Red;\"><b>Chú ý: Đây là email trả lời tự động. Nếu muốn phản hồi - Quý khách vui lòng gửi email về địa chỉ " + WebResource.GetValue("Web_Email") + " hoặc liên hệ trực tiếp với công ty.</b> </span></p></center>";
                if (item.ID > 0)
                {
                    var listEmail = WebResource.GetValue("Web_Email");
                    if (!string.IsNullOrEmpty(item.Email))
                    {
                        listEmail += (!string.IsNullOrEmpty(listEmail) ? ", " : "") + item.Email.Trim();
                    }
                    if (!string.IsNullOrEmpty(listEmail))
                    {
                        string city = "";
                        var mcity = item.GetCity();
                        var mdistrict = item.GetDistrict();
                        var mward = item.GetWard();
                        city = (mcity != null ? mcity.Name : "") + " - " + (mdistrict != null ? mdistrict.Name : "") + " - " + (mward != null ? mward.Name : "");

                        sHtml += "<table width=\"100%\" cellpadding=\"3px\" cellspacing=\"0\" style=\"font-family: Tahoma,Geneva,sans-serif; font-size: 12px; line-height: 18px\">";
                        sHtml += "<tbody><tr>";
                        sHtml += "<td align=\"center\" style=\"background-color: #f5f5f5\">";
                        sHtml += "<table bgcolor=\"#ffffff\" border=\"0\" cellpadding=\"18\" cellspacing=\"0\" width=\"100%\">";
                        sHtml += "<tbody><tr><td>";
                        sHtml += "<table width=\"100%\" cellpadding=\"0\" cellspacing=\"0\">";
                        sHtml += "<tbody><tr>";
                        sHtml += "<td width=\"50%\" style=\"text-align: left\"><a href=\"" + VSW.Core.Web.HttpRequest.Domain + "\" target=\"_blank\" >";
                        sHtml += "<img alt=\"ascpart.vn\" src=\"" + VSW.Core.Web.HttpRequest.Domain + "/Data/upload/images/Adv/logo.png\" style=\"max-width: 150px;\" class=\"CToWUd\"></a></td>";
                        sHtml += "<td width=\"50%\" style=\"text-align: right\">&nbsp;</td>";
                        sHtml += "</tr></tbody></table><hr>";
                        sHtml += "<p style=\"font-weight: bold\">Kính chào quý khách: " + item.Name + "</p>";
                        sHtml += "<p>Quý khách vừa đặt mua hàng tại website <a href=\"" + VSW.Core.Web.HttpRequest.Domain + "\" style=\"text-decoration: underline\" target=\"_blank\" >" + ViewPage.Request.Url.Host + "</a> với thông tin sau: </p>";
                        sHtml += "<table width=\"100%\" border=\"0\" cellpadding=\"5px\" cellspacing=\"0\">";
                        sHtml += "<tbody>";
                        sHtml += "<tr><td width=\"250\">Mã đơn hàng:</td><td><b>" + item.Code + "</b></td></tr>";
                        sHtml += "<tr><td>Ngày đặt mua:</td><td>" + string.Format("{0:dd-MM-yyyy HH:mm}", item.Created) + "</td></tr>";
                        sHtml += "<tr><td colspan=\"2\" style=\"background: #f5f5f5;\">Thông tin phí</td></tr>";
                        sHtml += "<tr>";
                        sHtml += "<td><span style=\"font-weight: bold;  font-style: italic;\">Phí đơn hàng:</span></td>";
                        sHtml += "<td><span style=\"font-weight: bold; font-style: italic;\">" + Utils.FormatMoney(item.Total) + " VNĐ</span></td>";
                        sHtml += "</tr>";
                        if (item.Fee > 0)
                        {
                            sHtml += "<tr>";
                            sHtml += "<td><span style=\"font-weight: bold;  font-style: italic\">Phí vận chuyển:</span></td>";
                            sHtml += "<td><span style=\"font-weight: bold; font-style: italic\">" + Utils.FormatMoney(item.Fee) + " VNĐ</span></td>";
                            sHtml += "</tr>";
                        }
                        if (item.SaleMoney > 0)
                        {
                            sHtml += "<tr>";
                            sHtml += "<td><span style=\"font-weight: bold;  font-style: italic\">Giảm giá:</span></td>";
                            sHtml += "<td><span style=\"font-weight: bold; font-style: italic\">Mã giảm giá: " + item.SaleCode + " - " + Utils.FormatMoney(item.SaleMoney) + " VNĐ</span></td>";
                            sHtml += "</tr>";
                        }
                        sHtml += "<tr>";
                        sHtml += "<td><span style=\"font-weight: bold;  font-style: italic;color:red;\">Tổng phí:</span></td>";
                        sHtml += "<td><span style=\"font-weight: bold; font-style: italic;color:red;\">" + Utils.FormatMoney(item.Total + item.Fee - item.SaleMoney - item.SaleCustomer - item.SaleMoneyPoint) + " VNĐ</span></td>";
                        sHtml += "</tr>";
                        sHtml += "<tr><td colspan=\"2\" style=\"background: #f5f5f5;\">Thông tin thanh toán</td></tr>";
                        sHtml += "<tr><td><span>Hình thức thanh toán:</span></td><td>" + (item.Payment == 3 ? ("Thanh toán trực tuyến - " + item.BankPay) : (item.Payment == 2 ? ("Chuyển khoản qua ngân hàng") : "Thanh toán khi nhận hàng")) + " </td></tr>";
                        sHtml += "<tr><td colspan=\"2\" style=\"background: #f5f5f5;\">Thông tin khách hàng</td></tr>";
                        sHtml += "<tr><td><span>Họ và tên:</span></td><td>" + item.Name + "</td></tr>";
                        sHtml += "<tr><td><span>Địa chỉ:</span></td><td>" + city + "-" + item.Address + "</td></tr>";
                        sHtml += "<tr><td><span>Email:</span></td><td><a href=\"mailto:" + item.Email + "\" target=\"_blank\">" + item.Email + "</a> </td></tr>";
                        sHtml += "<tr><td><span>Số điện thoại:</span></td><td>" + item.Phone + "</td></tr>";
                        if (item.AddressMore)
                        {
                            string city2 = "";
                            var mcity2 = item.GetCity2();
                            var mdistrict2 = item.GetDistrict2();
                            var mward2 = item.GetWard2();
                            city2 = (mcity2 != null ? mcity2.Name : "") + " - " + (mdistrict2 != null ? mdistrict2.Name : "") + " - " + (mward2 != null ? mward2.Name : "");
                            sHtml += "<tr><td colspan=\"2\" style=\"background: #f5f5f5;\">Thông tin giao hàng</td></tr>";
                            sHtml += "<tr><td><span>Địa chỉ:</span></td><td>" + city2 + "-" + item.Address2 + "</td></tr>";
                        }
                        sHtml += "<tr><td colspan=\"2\" style=\"background: #f5f5f5;\">Thông tin đơn hàng</td></tr>";
                        sHtml += "<tr><td colspan=\"2\">";
                        sHtml += @"<table cellspacing=""0"" border=""1"" style=""border:0.1px solid black;border-collapse:collapse;width:100%;margin-right:15px""> ";
                        sHtml += @"<tbody>";
                        sHtml += @"<tr style=""height:27px"">";
                        sHtml += @"<td style=""border-top:1px solid #000000;border-bottom:1px solid #000000;border-left:1px solid #000000;border-right:1px solid #000000"" align=""center"" valign=""middle"" bgcolor=""#D6D6C2"" width=""3%""><b><font face=""Arial"" size=""1"" color=""#1C1C1C"">STT</font></b></td>";
                        sHtml += @"<td style=""border-top:1px solid #000000;border-bottom:1px solid #000000;border-left:1px solid #000000;border-right:1px solid #000000"" align=""center"" valign=""middle"" bgcolor=""#D6D6C2"" width=""31%""><b><font face=""Arial"" size=""1"" color=""#1C1C1C"">Tên sản phẩm</font></b></td>";
                        sHtml += @"<td style=""border-top:1px solid #000000;border-bottom:1px solid #000000;border-left:1px solid #000000;border-right:1px solid #000000"" align=""center"" valign=""middle"" bgcolor=""#D6D6C2"" width=""20%""><b><font face=""Arial"" size=""1"" color=""#1C1C1C"">Thông tin</font></b></td>";
                        sHtml += @"<td style=""border-top:1px solid #000000;border-bottom:1px solid #000000;border-left:1px solid #000000;border-right:1px solid #000000"" align=""center"" valign=""middle"" bgcolor=""#D6D6C2"" width=""18%""><b><font face=""Arial"" size=""1"" color=""#1C1C1C"">Giá</font></b></td>";
                        sHtml += @"<td style=""border-top:1px solid #000000;border-bottom:1px solid #000000;border-left:1px solid #000000;border-right:1px solid #000000"" align=""center"" valign=""middle"" bgcolor=""#D6D6C2"" width=""8%""><b><font face=""Arial"" size=""1"" color=""#1C1C1C"">Số lượng</font></b></td>";
                        sHtml += @"<td style=""border-top:1px solid #000000;border-bottom:1px solid #000000;border-left:1px solid #000000;border-right:1px solid #000000"" align=""center"" valign=""middle"" bgcolor=""#D6D6C2"" width=""18%""><b><font face=""Arial"" size=""1"" color=""#1C1C1C"">Thành tiền</font></b></td>";
                        sHtml += @"</tr>";
                        sHtml += html_details;
                        sHtml += @"</tbody></table>";
                        sHtml += "</td></tr>";
                        sHtml += "<tr><td>Lời nhắn của Quý khách:</td><td>" + item.Content + "</td></tr>";
                        sHtml += "</tbody></table>";
                        sHtml += "<p></p>";
                        sHtml += "<hr>";
                        sHtml += WebResource.GetValue("NoteInfoFeedback");
                        sHtml += "</td></tr></tbody></table>";
                        sHtml += "</td></tr></tbody></table>";

                        var domain = ViewPage.Request.Url.Host;
                        string fromemail = VSW.Core.Global.Config.GetValue("Mod.SmtpUser").ToString();
                        //gui mail cho quan tri va khach hang
                        Mail.SendMail(
                            listEmail,
                            fromemail,
                            domain,
                            domain + "- Thông tin đơn hàng - ngày " + $"{DateTime.Now:dd/MM/yyyy HH:mm}",
                            sHtml
                        );
                    }
                }

                #endregion
                cart.RemoveAll();
                cart.Save();
                ViewPage.Response.Redirect(ViewPage.CompleteUrl + "?OrderID=" + item.ID + "&returnpath=" + model.returnpath);

            }
        }

        #region private
        private static string GetOrder(int orderid)
        {
            if (orderid <= 1) return "0000001";

            var result = string.Empty;
            for (var i = 1; i <= (7 - orderid.ToString().Length); i++)
            {
                result += "0";
            }

            return result + (orderid + 1);
        }
        #endregion private
    }

    public class MViewCartModel
    {
        public int Index { get; set; }
        public int ProductID { get; set; }
        public int SizeID { get; set; }
        public int ColorID { get; set; }

        private int _quantity = 1;
        public int Quantity
        {
            get { return _quantity; }
            set { _quantity = value; }
        }

        public int[] ArrQuantity { get; set; }
        public string returnpath { get; set; }
        public int AnhChi { get; set; }
        public bool HasTraGopBaoKim { get; set; }
    }
}