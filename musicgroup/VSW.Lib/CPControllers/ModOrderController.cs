using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Đơn hàng",
        Description = "Quản lý  - Đơn hàng",
        Code = "ModOrder",
        Access = 31,
        Order = 3,
        ShowInMenu = true,
        CssClass = "icon-16-article")]
    public class ModOrderController : CPController
    {
        public ModOrderController()
        {
            //khoi tao Service
            DataService = ModOrderService.Instance;
            CheckPermissions = true;
        }

        public void ActionIndex(ModOrderModel model)
        {
            // sap xep tu dong
            var orderBy = AutoSort(model.Sort);
            var FromDate = DateTime.MinValue;
            var ToDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(model.FromDate))
            {
                FromDate = VSW.Core.Global.Convert.ToDateTime(model.FromDate + " 00:00:00");
            }
            if (!string.IsNullOrEmpty(model.ToDate))
            {
                ToDate = VSW.Core.Global.Convert.ToDateTime(model.ToDate + " 23:59:59");
            }
            // tao danh sach
            var dbQuery = ModOrderService.Instance.CreateQuery()
                                    .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText) || o.Email.Contains(model.SearchText) || o.Phone.Contains(model.SearchText) || o.Address.Contains(model.SearchText)))
                                    .Where(FromDate > DateTime.MinValue, o => o.Created >= FromDate)
                                    .Where(ToDate > DateTime.MinValue, o => o.Created <= ToDate)
                                    .Where(model.StatusID > 0, o => o.StatusID == model.StatusID)
                                    .Take(model.PageSize)
                                    .OrderBy(orderBy)
                                    .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModOrderModel model)
        {
            if (model.RecordID > 0)
            {
                _item = ModOrderService.Instance.GetByID(model.RecordID);
                model.ChangeStatus = _item.StatusID;
                model.ShowroomOld = _item.ReceiveValue;
                if (_item.OrderNews)
                {
                    _item.OrderNews = false;
                    ModOrderService.Instance.Save(_item, o => o.OrderNews);
                }
                // khoi tao gia tri mac dinh khi update
            }
            else
            {
                _item = new ModOrderEntity();
                _item.OrderNews = false;
                // khoi tao gia tri mac dinh khi insert
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModOrderModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModOrderModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }
        public void ActionExport(ModOrderModel model)
        {
            var orderBy = AutoSort(model.Sort);
            var FromDate = DateTime.MinValue;
            var ToDate = DateTime.MinValue;
            if (!string.IsNullOrEmpty(model.FromDate))
            {
                FromDate = VSW.Core.Global.Convert.ToDateTime(model.FromDate + " 00:00:00");
            }
            if (!string.IsNullOrEmpty(model.ToDate))
            {
                ToDate = VSW.Core.Global.Convert.ToDateTime(model.ToDate + " 23:59:59");
            }
            var listItem = ModOrderService.Instance.CreateQuery()
                        .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(model.SearchText) || o.Email.Contains(model.SearchText) || o.Phone.Contains(model.SearchText) || o.Address.Contains(model.SearchText)))
                        .Where(FromDate > DateTime.MinValue, o => o.Created >= FromDate)
                        .Where(ToDate > DateTime.MinValue, o => o.Created <= ToDate)
                        .Where(model.StatusID > 0, o => o.StatusID == model.StatusID)
                        .OrderBy(orderBy)
                        .ToList_Cache();

            var listExcel = new List<List<object>>();
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                var Status = listItem[i].GetStatus();
                var listRow = new List<object>
                {
                    (i+1),
                    listItem[i].Code,
                    listItem[i].Name,
                    listItem[i].Phone,
                    listItem[i].Email,
                    listItem[i].Address+"-"+listItem[i].GetWard()?.Name+"-"+listItem[i].GetDistrict()?.Name+"-"+listItem[i].GetCity()?.Name,
                    Utils.FormatMoney(listItem[i].Total + listItem[i].Fee - listItem[i].SaleMoney - listItem[i].SaleCustomer - listItem[i].SaleMoneyPoint),
                    (Status != null ? Status.Name : ""),
                    Utils.GetNameByConfigkey("Mod.Payment", listItem[i].Payment) +(!string.IsNullOrEmpty(listItem[i].BankPay) ? " - "+listItem[i].BankPay+(listItem[i].StatusPay ? " - Đã thanh toán" : " - Chưa thanh toán") : ""),
                    Utils.GetNameByConfigkey("Mod.Receive", listItem[i].Receive)
                };

                listExcel.Add(listRow);
            }

            string exportFile = CPViewPage.Server.MapPath("~/Data/upload/files/EXPORT/DH_" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "_" + DateTime.Now.Ticks + ".xls");

            Excel.Export(listExcel, 6, CPViewPage.Server.MapPath("~/CP/Template/TempDownloadDH.xlsx"), exportFile);

            CPViewPage.Response.Clear();
            CPViewPage.Response.ContentType = "application/excel";
            CPViewPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(exportFile));
            CPViewPage.Response.WriteFile(exportFile);
            CPViewPage.Response.End();
        }
        #region private func

        private ModOrderEntity _item;

        private bool ValidSave(ModOrderModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            //kiem tra quyen han
            if ((model.RecordID < 1 && !CPViewPage.UserPermissions.Add) || (model.RecordID > 0 && !CPViewPage.UserPermissions.Edit))
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");

            if (CPViewPage.Message.ListMessage.Count != 0) return false;

            //save
            ModOrderService.Instance.Save(_item);

            return true;
        }
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
        #endregion private func
    }

    public class ModOrderModel : DefaultModel
    {
        private int _langID = 1;

        public int LangID
        {
            get { return _langID; }
            set { _langID = value; }
        }

        public string SearchText { get; set; }
        public int ChangeStatus { get; set; }
        public int Customer { get; set; }
        public int CountBuy { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int StatusID { get; set; }
        public int ShowroomOld { get; set; }
        public string ProductDelete { get; set; }
    }
}