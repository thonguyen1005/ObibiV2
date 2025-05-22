using System;
using System.Collections.Generic;
using System.Web;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Khách hàng",
        Description = "Quản lý - Khách hàng",
        Code = "ModWebUser",
        Access = 31,
        Order = 8,
        ShowInMenu = true,
        CssClass = "user")]
    public class ModWebUserController : CPController
    {
        public ModWebUserController()
        {
            //khoi tao Service
            DataService = ModWebUserService.Instance;
            //CheckPermissions = false;
        }

        public void ActionIndex(ModWebUserModel model)
        {
            //sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            //tao danh sach
            var dbQuery = ModWebUserService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(Data.GetCode(model.SearchText)) || o.Phone.Contains(model.SearchText) || o.Email.Contains(model.SearchText)))
                                .Where(model.Type > 0, o => o.Type == model.Type)
                                .Where(model.Type2 > 0, o => o.Type2 == model.Type2)
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }

        public void ActionAdd(ModWebUserModel model)
        {
            if (model.RecordID > 0)
            {
                _item = model.RecordID > 0 ? ModWebUserService.Instance.GetByID(model.RecordID) : new ModWebUserEntity();
            }
            else
            {
                //khoi tao gia tri mac dinh khi insert
                _item = new ModWebUserEntity
                {
                    Created = DateTime.Now,
                    Activity = CPViewPage.UserPermissions.Approve
                };
            }

            ViewBag.Data = _item;
            ViewBag.Model = model;
        }

        public void ActionSave(ModWebUserModel model)
        {
            if (ValidSave(model))
                SaveRedirect();
        }

        public void ActionApply(ModWebUserModel model)
        {
            if (ValidSave(model))
                ApplyRedirect(model.RecordID, _item.ID);
        }

        public void ActionSaveNew(ModWebUserModel model)
        {
            if (ValidSave(model))
                SaveNewRedirect(model.RecordID, _item.ID);
        }
        public override void ActionDelete(int[] arrID)
        {
            //xoa cleanurl
            ModWebUserFileService.Instance.Delete("[WebUserID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //xoa News
            DataService.Delete("[ID] IN (" + Core.Global.Array.ToString(arrID) + ")");

            //thong bao
            CPViewPage.SetMessage("Đã xóa thành công.");
            CPViewPage.RefreshPage();
        }

        public void ActionExport(ModWebUserModel model)
        {
            var listItem = ModWebUserService.Instance.CreateQuery()
                            .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Name.Contains(model.SearchText) || o.Code.Contains(Data.GetCode(model.SearchText)) || o.Phone.Contains(model.SearchText) || o.Email.Contains(model.SearchText)))
                            .Where(model.Type > 0, o => o.Type == model.Type)
                            .Where(model.Type2 > 0, o => o.Type2 == model.Type2)
                            .ToList_Cache();

            var listExcel = new List<List<object>>();
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                int total = ModOrderService.Instance.CreateQuery()
                                                            .Where(o => o.WebUserID == listItem[i].ID)
                                                            .Count()
                                                            .ToValue().ToInt(0);

                int done = ModOrderService.Instance.CreateQuery()
                            .Where(o => o.WebUserID == listItem[i].ID && o.StatusID == 11981)
                            .Count()
                            .ToValue().ToInt(0);
                int cancel = ModOrderService.Instance.CreateQuery()
                            .Where(o => o.WebUserID == listItem[i].ID && o.StatusID == 11982)
                            .Count()
                            .ToValue().ToInt(0);

                string sql = " select sum(Total) from [Mod_Order] where WebUserID = " + listItem[i].ID;
                long totalMoney = ModOrderService.Instance.ExecuteScalar(sql).ToLong(0);
                string sql2 = " select sum(Total) from [Mod_Order] where WebUserID = " + listItem[i].ID + " And StatusID = 11981";
                long totalMoney2 = ModOrderService.Instance.ExecuteScalar(sql2).ToLong(0);

                var listRow = new List<object>
                {
                    (i+1),
                    (listItem[i].Type2 > 0 ? (listItem[i].WebUserMenu?.Name??"") : ""),
                    listItem[i].Name,
                    listItem[i].Email,
                    listItem[i].Phone,
                    listItem[i].Address+"-"+listItem[i].GetWard()?.Name+"-"+listItem[i].GetDistrict()?.Name+"-"+listItem[i].GetCity()?.Name,
                    Utils.FormatMoney(total),
                    Utils.FormatMoney(done),
                    Utils.FormatMoney(cancel),
                    Utils.FormatMoney(VSW.Core.Global.Convert.ToDouble(done)/(total == 0 ? 1 : VSW.Core.Global.Convert.ToDouble(total))*VSW.Core.Global.Convert.ToDouble(100)),
                    Utils.FormatMoney(totalMoney),
                    Utils.FormatMoney(totalMoney2)
                };

                listExcel.Add(listRow);
            }

            string exportFile = CPViewPage.Server.MapPath("~/Data/upload/files/EXPORT/KH_" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "_" + DateTime.Now.Ticks + ".xls");

            Excel.Export(listExcel, 6, CPViewPage.Server.MapPath("~/CP/Template/TempDownloadKH.xlsx"), exportFile);

            CPViewPage.Response.Clear();
            CPViewPage.Response.ContentType = "application/excel";
            CPViewPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(exportFile));
            CPViewPage.Response.WriteFile(exportFile);
            CPViewPage.Response.End();
        }
        #region private func

        private ModWebUserEntity _item;

        private bool ValidSave(ModWebUserModel model)
        {
            TryUpdateModel(_item);

            ViewBag.Data = _item;
            ViewBag.Model = model;

            CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;

            if (_item.UserName.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập: Tên đăng nhập.");
            else if (_item.UserName != Data.RemoveVietNamese(_item.UserName) || _item.UserName.ToLower() != ModWebUserService.Instance.GetUserName(_item.UserName).ToLower())
                CPViewPage.Message.ListMessage.Add("Tên đăng nhập phải viết liền không dấu.");
            else if (_item.UserName.Trim().Length < 3)
                CPViewPage.Message.ListMessage.Add("Tên đăn nhập phải tối thiểu 3 ký tự.");
            else if (ModWebUserService.Instance.CheckUserName(_item.UserName, _item.ID))
                CPViewPage.Message.ListMessage.Add("Tài khoản đăng nhập đã tồn tại.");

            if (_item.ID < 1)
            {
                if (model.Password2.Trim() == string.Empty)
                    CPViewPage.Message.ListMessage.Add("Nhập: Mật khẩu.");
            }

            if (_item.Name.Trim() == string.Empty)
                CPViewPage.Message.ListMessage.Add("Nhập: Họ và tên.");
            if (!string.IsNullOrEmpty(_item.Email.Trim()))
            {
                if (!Global.Utils.IsEmailAddress(_item.Email.Trim()))
                    CPViewPage.Message.ListMessage.Add("Nhập: Địa chỉ email.");
                else if (ModWebUserService.Instance.CheckEmail(_item.Email.Trim(), _item.ID))
                    CPViewPage.Message.ListMessage.Add("Email đã tồn tại. Hãy chọn email khác.");
            }

            if (CPViewPage.Message.ListMessage.Count == 0)
            {
                if (model.Password2 != string.Empty)
                    _item.Password = Security.Md5(model.Password2);

                try
                {
                    _item.IP = VSW.Core.Web.HttpRequest.IP;
                    _item.Created = DateTime.Now;
                    //save
                    ModWebUserService.Instance.Save(_item);
                }
                catch (Exception ex)
                {
                    Error.Write(ex);
                    CPViewPage.Message.ListMessage.Add(ex.Message);
                    return false;
                }

                return true;
            }

            return false;
        }

        #endregion private func
    }

    public class ModWebUserModel : DefaultModel
    {
        public string SearchText { get; set; }
        public string Password2 { get; set; }
        public int Type { get; set; }
        public int Type2 { get; set; }
        public int WebUserID { get; set; }
    }
}