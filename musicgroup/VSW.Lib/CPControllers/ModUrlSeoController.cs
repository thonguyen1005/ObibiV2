using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Script.Serialization;
using VSW.Lib.Global;
using VSW.Lib.Models;
using VSW.Lib.MVC;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "SEO đường dẫn",
        Description = "Quản lý - SEO đường dẫn",
        Code = "ModUrlSeo",
        Access = 31,
        Order = 4,
        ShowInMenu = true,
        CssClass = "google-plus-square")]
    public class ModUrlSeoController : CPController
    {
        public ModUrlSeoController()
        {
            //khoi tao Service
            DataService = ModUrlSeoService.Instance;
            DataEntity = new ModUrlSeoEntity();
            CheckPermissions = true;
        }

        public void ActionIndex(ModUrlSeoModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);
            //tao danh sach
            var dbQuery = ModUrlSeoService.Instance.CreateQuery()
                                .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Url.Contains(model.SearchText) || o.UrlRedirect.Contains(model.SearchText)))
                                .Where(o => o.Activity == false)
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }
        public void ActionExport(ModUrlSeoModel model)
        {
            //sap xep tu dong
            var orderBy = AutoSort(model.Sort);
            var listItem = ModUrlSeoService.Instance.CreateQuery()
                               .Where(!string.IsNullOrEmpty(model.SearchText), o => (o.Url.Contains(model.SearchText)))
                               .OrderBy(orderBy)
                               .ToList_Cache();

            var listExcel = new List<List<object>>();
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                var listRow = new List<object>
                {
                    (i+1),
                    listItem[i].ID,
                    listItem[i].Url,
                    listItem[i].UrlRedirect,
                    "",
                    "",
                    ""
                };

                listExcel.Add(listRow);
            }

            string exportFile = CPViewPage.Server.MapPath("~/Data/upload/files/EXPORT/UrlSEO_" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "_" + DateTime.Now.Ticks + ".xls");

            Excel.Export(listExcel, 6, CPViewPage.Server.MapPath("~/CP/Template/TempUrlSEO.xlsx"), exportFile);

            CPViewPage.Response.Clear();
            CPViewPage.Response.ContentType = "application/excel";
            CPViewPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(exportFile));
            CPViewPage.Response.WriteFile(exportFile);
            CPViewPage.Response.End();
        }
        public override void ActionPublish(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")", "@Activity", 1);

            for (int i = 0; arrID != null && i < arrID.Length; i++)
            {
                if (arrID[i] < 1) continue;
                var UrlSeo = ModUrlSeoService.Instance.GetByID_Cache(arrID[i]);
                if (UrlSeo == null) continue;

                if (!ModUrlSeoActivityService.Instance.CheckUrl(UrlSeo.Url))
                {
                    ModUrlSeoActivityService.Instance.Save(
                        new ModUrlSeoActivityEntity()
                        {
                            ID = 0,
                            Url = UrlSeo.Url,
                            UrlRedirect = UrlSeo.UrlRedirect,
                            CountSearch = UrlSeo.CountSearch
                        }
                    );
                }
            }

            DataService.Delete("[ID] IN (" + VSW.Core.Global.Array.ToString(arrID) + ")");
            //thong bao
            CPViewPage.SetMessage("Đã duyệt thành công. Bạn hãy vào phần quản lý url chuẩn hóa cập nhật dữ liệu.");
            CPViewPage.RefreshPage();
        }

        public override void ActionPublishGX(int[] arrID)
        {
            if (CheckPermissions && !CPViewPage.UserPermissions.Approve)
            {
                //thong bao
                CPViewPage.Message.MessageType = Message.MessageTypeEnum.Error;
                CPViewPage.Message.ListMessage.Add("Quyền hạn chế.");
                return;
            }

            DataService.Update("[ID]=" + arrID[0], "@Activity", arrID[1]);
            if (arrID[1] == 1)
            {
                var UrlSeo = ModUrlSeoService.Instance.GetByID_Cache(arrID[0]);
                if (UrlSeo != null)
                {
                    if (!ModUrlSeoActivityService.Instance.CheckUrl(UrlSeo.Url))
                    {
                        ModUrlSeoActivityService.Instance.Save(
                            new ModUrlSeoActivityEntity()
                            {
                                ID = 0,
                                Url = UrlSeo.Url,
                                UrlRedirect = UrlSeo.UrlRedirect,
                                CountSearch = UrlSeo.CountSearch
                            }
                        );
                    }
                }

                DataService.Delete("[ID] = " + arrID[0] + "");
            }
            //thong bao
            CPViewPage.SetMessage(arrID[1] == 0 ? "Đã bỏ duyệt thành công." : "Đã duyệt thành công. Bạn hãy vào phần quản lý url chuẩn hóa cập nhật dữ liệu.");
            CPViewPage.RefreshPage();
        }
    }

    public class ModUrlSeoModel : DefaultModel
    {
        public int LangID { get; set; } = 1;
        public string SearchText { get; set; }
        public string Excel { get; set; }
    }
}