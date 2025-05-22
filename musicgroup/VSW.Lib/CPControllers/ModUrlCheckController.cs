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
    [CPModuleInfo(Name = "Kiểm tra Url",
        Description = "Quản lý - Kiểm tra Url",
        Code = "ModUrlCheck",
        Access = 31,
        Order = 4,
        ShowInMenu = true,
        CssClass = "google-plus-square")]
    public class ModUrlCheckController : CPController
    {
        public ModUrlCheckController()
        {
            CheckPermissions = true;
        }

        public void ActionIndex(ModUrlCheckModel model)
        {
            ViewBag.Model = model;
        }
        public void ActionExport(ModUrlCheckModel model)
        {
            string importFile = CPViewPage.Server.MapPath(model.Excel);
            var listItem = ImportUrlSEO(importFile);

            var listExcel = new List<List<object>>();
            for (int i = 0; listItem != null && i < listItem.Count; i++)
            {
                var listRow = new List<object>
                {
                    (i+1),
                    listItem[i].Address,
                    listItem[i].Content,
                    listItem[i].Status_Code,
                    listItem[i].Status,
                    listItem[i].Indexability,
                    listItem[i].Indexability_Status,
                    listItem[i].Hash,
                    listItem[i].Length,
                    listItem[i].Canonical,
                    listItem[i].URL_Encoded,
                    listItem[i].Type
                };

                listExcel.Add(listRow);
            }

            string exportFile = CPViewPage.Server.MapPath("~/Data/upload/files/EXPORT/UrlCheck_" + string.Format("{0:ddMMyyyy}", DateTime.Now) + "_" + DateTime.Now.Ticks + ".xls");

            Excel.Export(listExcel, 2, CPViewPage.Server.MapPath("~/CP/Template/TempUrlCheck.xlsx"), exportFile);

            CPViewPage.Response.Clear();
            CPViewPage.Response.ContentType = "application/excel";
            CPViewPage.Response.AppendHeader("Content-Disposition", "attachment; filename=" + System.IO.Path.GetFileName(exportFile));
            CPViewPage.Response.WriteFile(exportFile);
            CPViewPage.Response.End();
        }
        public void ActionImport(ModUrlCheckModel model)
        {
            string importFile = CPViewPage.Server.MapPath(model.Excel);
            var result = ImportUrlSEO(importFile);
            ViewBag.Data = result;
            ViewBag.Model = model;
        }

        public List<ModUrlCheckEntity> ImportUrlSEO(string file)
        {
            var list = new List<ModUrlCheckEntity>();
            if (!file.EndsWith(".xls") && !file.EndsWith(".xlsx")) return list;

            var fstream = new FileStream(file, FileMode.Open);
            var workbook = new Workbook();
            workbook.Open(fstream);
            var worksheet = workbook.Worksheets[0];
            if (worksheet == null || worksheet.Cells.MaxRow < 1) return list;
            for (int i = 2; i <= worksheet.Cells.MaxRow; i++)
            {
                var item = new ModUrlCheckEntity();
                item.Address = (worksheet.Cells[i, 0].StringValue.Trim());
                item.Content = (worksheet.Cells[i, 1].StringValue.Trim());
                item.Status_Code = (worksheet.Cells[i, 2].StringValue.Trim());
                item.Status = (worksheet.Cells[i, 3].StringValue.Trim());
                item.Indexability = (worksheet.Cells[i, 4].StringValue.Trim());
                item.Indexability_Status = (worksheet.Cells[i, 5].StringValue.Trim());
                item.Hash = (worksheet.Cells[i, 6].StringValue.Trim());
                item.Length = (worksheet.Cells[i, 7].StringValue.Trim());
                item.Canonical = (worksheet.Cells[i, 8].StringValue.Trim());
                item.URL_Encoded = (worksheet.Cells[i, 9].StringValue.Trim());

                string fullUrl = item.Address;
                if (string.IsNullOrEmpty(fullUrl)) break;
                string[] arrUrl = fullUrl.Split('/');
                string end = arrUrl[arrUrl.Length - 1].Trim();
                if (string.IsNullOrEmpty(end)) item.Type = "Page";
                else if (end.ToLower().Contains(".js")) item.Type = "JS";
                else if (end.ToLower().Contains(".css")) item.Type = "Css";
                else if (end.ToLower().Contains(".png")) item.Type = "Image";
                else if (end.ToLower().Contains(".jpg")) item.Type = "Image";
                else if (end.ToLower().Contains(".webp")) item.Type = "Image";
                else if (item.Content.ToLower().Contains("image")) item.Type = "Image";
                else
                {
                    var cleanUrl = ModCleanURLService.Instance.GetByCode(end, 1);
                    item.Type = cleanUrl?.Type;
                }
                list.Add(item);
            }

            fstream.Close();
            fstream.Dispose();
            return list;
        }
    }

    public class ModUrlCheckModel : DefaultModel
    {
        public string Excel { get; set; }
    }
}