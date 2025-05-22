using System;
using System.Collections.Generic;

using VSW.Lib.MVC;
using VSW.Lib.Models;
using VSW.Lib.Global;

namespace VSW.Lib.CPControllers
{
    [CPModuleInfo(Name = "Email đăng ký",
        Description = "Quản lý - Email đăng ký",
        Code = "ModSubscribe",
        Access = 9,
        Order = 9900,
        ShowInMenu = true,
        CssClass = "envelope-square")]
    public class ModSubscribeController : CPController
    {
        public ModSubscribeController()
        {
            //khoi tao Service
            DataService = ModSubscribeService.Instance;
            CheckPermissions = true;
        }
        public void ActionExport(ModSubscribeModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort);
            // tao danh sach
            var listItem = ModSubscribeService.Instance.CreateQuery()
                                .OrderBy(orderBy)
                                    .ToList_Cache();

            var listExcel = new List<List<object>>();
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                var listRow = new List<object>
                {
                    (i+1),
                    listItem[i].Email
                };

                listExcel.Add(listRow);
            }

            string exportFile = CPViewPage.Server.MapPath("~/Data/upload/files/EXPORT/Email_" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "_" + DateTime.Now.Ticks + ".xls");

            Excel.Export(listExcel, 6, CPViewPage.Server.MapPath("~/CP/Template/TempDownloadEmail.xlsx"), exportFile);

            CPViewPage.Response.Clear();
            CPViewPage.Response.ContentType = "application/excel";
            CPViewPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(exportFile));
            CPViewPage.Response.WriteFile(exportFile);
            CPViewPage.Response.End();
        }
        public void ActionIndex(ModSubscribeModel model)
        {
            // sap xep tu dong
            string orderBy = AutoSort(model.Sort);

            // tao danh sach
            var dbQuery = ModSubscribeService.Instance.CreateQuery()
                                .Take(model.PageSize)
                                .OrderBy(orderBy)
                                .Skip(model.PageIndex * model.PageSize);

            ViewBag.Data = dbQuery.ToList();
            model.TotalRecord = dbQuery.TotalRecord;
            ViewBag.Model = model;
        }
    }

    public class ModSubscribeModel : DefaultModel
    {

    }
}
