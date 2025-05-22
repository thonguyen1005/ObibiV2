using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Aspose.Cells;
using VSW.Lib.Models;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace VSW.Lib.Global
{
    public class Excel
    {
        public static void Export(List<List<object>> list, int start_row, string sourceFile, string exportFile)
        {
            if (list == null) return;

            Workbook workbook = new Workbook();
            workbook.Open(sourceFile);
            Cells cells = workbook.Worksheets[0].Cells;

            for (int i = 0; i < list.Count; i++)
            {
                for (int j = 0; j < list[i].Count; j++)
                {
                    Style style = cells[start_row + i, j].GetStyle();
                    style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
                    style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
                    cells[start_row + i, j].SetStyle(style);
                    cells[start_row + i, j].PutValue(list[i][j]);
                }
            }

            workbook.Save(exportFile);
        }
        public static string ImportProduct(string file, int check = 0)
        {
            if (!file.EndsWith(".xls") && !file.EndsWith(".xlsx"))
            {
                return "File không hợp lệ. Yêu cầu file Excel!";
            }


            var fstream = new FileStream(file, FileMode.Open);
            var workbook = new Workbook();
            workbook.Open(fstream);
            var worksheet = workbook.Worksheets[0];
            if (worksheet == null || worksheet.Cells.MaxRow < 1)
            {
                return "File không hợp lệ. Yêu cầu file Excel!";
            }
            int success = 0;
            int successUpdate = 0;
            string rowUpdate = "";
            string error = "";
            string rowerror = "";
            string Name = "";

            for (int i = 6; i <= worksheet.Cells.MaxRow; i++)
            {
                Name = worksheet.Cells[i, 1].StringValue.Trim();
                if (string.IsNullOrEmpty(Name)) continue;
                var item = new ModProductEntity();
                string model = worksheet.Cells[i, 2].StringValue.Trim().Replace("'", "");
                bool flag = false;
                if (check == 0 && !string.IsNullOrEmpty(model))
                {
                    var checkModel = ModProductService.Instance.GetByModel(model);
                    if (checkModel != null)
                    {
                        successUpdate++;
                        rowUpdate += (!string.IsNullOrEmpty(rowUpdate) ? "," : "") + (i + 1);
                        continue;
                    }
                }
                else if (check == 1 && !string.IsNullOrEmpty(model))
                {
                    var checkModel = ModProductService.Instance.GetByModel(model);
                    if (checkModel != null)
                    {
                        item = checkModel;
                    }
                }
                else if (check == 2 && !string.IsNullOrEmpty(model))
                {
                    var checkModel = ModProductService.Instance.GetByModel(model);
                    if (checkModel != null)
                    {
                        flag = true;
                    }
                }
                if (check == 1)
                {
                    if (item.ID < 1) continue;
                }
                if (check == 2 && !flag) continue;
                item.Name = Name;
                item.Model = model;
                item.Weight = VSW.Core.Global.Convert.ToInt(worksheet.Cells[i, 3].StringValue.Trim().Replace(",", ""));
                item.Price = VSW.Core.Global.Convert.ToInt(worksheet.Cells[i, 4].StringValue.Trim().Replace(",", ""));
                item.Price2 = VSW.Core.Global.Convert.ToInt(worksheet.Cells[i, 5].StringValue.Trim().Replace(",", ""));
                item.PricePromotion = VSW.Core.Global.Convert.ToInt(worksheet.Cells[i, 6].StringValue.Trim().Replace(",", ""));
                item.Price = item.Price < 0 ? 0 : item.Price;
                item.Price2 = item.Price2 < 0 ? 0 : item.Price2;
                item.Weight = item.Weight < 0 ? 0 : item.Weight;
                item.PricePromotion = item.PricePromotion < 0 ? 0 : item.PricePromotion;
                item.DatePromotion = VSW.Core.Global.Convert.ToDateTime(worksheet.Cells[i, 7].StringValue.Trim());
                string _brandItem = worksheet.Cells[i, 8].StringValue.Trim();
                var brandID = 0;
                var _brand = WebMenuService.Instance.CreateQuery().Where(!string.IsNullOrEmpty(_brandItem), o => o.Type == "Brand" && o.Code == Data.GetCode(_brandItem)).ToSingle();
                if (!string.IsNullOrEmpty(_brandItem))
                {
                    if (_brand != null)
                    {
                        brandID = _brand.ID;
                    }
                    else
                    {
                        var newbrand = new WebMenuEntity();
                        newbrand.LangID = 1;
                        newbrand.Type = "Brand";
                        newbrand.Activity = true;
                        newbrand.Name = _brandItem;
                        newbrand.ParentID = 4;
                        newbrand.Code = Data.GetCode(newbrand.Name);
                        WebMenuService.Instance.Save(newbrand);
                        brandID = newbrand.ID;
                    }
                }
                item.BrandID = brandID;
                int menuID = VSW.Core.Global.Convert.ToInt(worksheet.Cells[i, 9].StringValue.Trim());
                item.MenuID = menuID;
                item.Star = 5;
                item.Summary = worksheet.Cells[i, 10].StringValue.Trim();
                item.Content = worksheet.Cells[i, 11].StringValue.Trim();
                item.Content = Regex.Replace(item.Content, "\r\n", "<br/>");
                item.Specifications = worksheet.Cells[i, 12].StringValue.Trim();
                item.Specifications = Regex.Replace(item.Specifications, "\r\n", "<br/>");
                string fileI = worksheet.Cells[i, 13].StringValue.Trim();
                string fileFolder = "";
                if (!string.IsNullOrEmpty(fileI))
                {
                    try
                    {
                        if (fileI.IndexOf(".") > -1)
                        {
                            fileI = fileI.Replace(@"\", "/");
                            if (!fileI.StartsWith("/")) fileI = fileI.Insert(0, "/");
                            fileI = fileI.Replace(@" ", "%20");
                            fileI = fileI.Replace(@"&", "%26");
                            item.File = fileI;
                        }
                        else
                        {
                            if (!fileI.StartsWith("/Data")) fileI = "/Data" + fileI;
                            fileFolder = fileI;
                            string[] fileEntries = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~" + fileFolder));
                            if (fileEntries != null && fileEntries.Length > 0)
                            {
                                FileInfo fi = new FileInfo(fileEntries[0]);
                                string extn = fi.Extension;
                                fileI += "/" + model + "(1)" + extn;
                            }
                            fileI = fileI.Replace(@" ", "%20");
                            fileI = fileI.Replace(@"&", "%26");
                            item.File = fileI;
                        }
                    }
                    catch { item.File = ""; }
                }

                item.PageTitle = worksheet.Cells[i, 15].StringValue.Trim();
                item.PageDescription = worksheet.Cells[i, 16].StringValue.Trim();
                item.PageKeywords = worksheet.Cells[i, 17].StringValue.Trim();
                string listfile = worksheet.Cells[i, 14].StringValue.Trim();
                string files = "";
                if (!string.IsNullOrEmpty(listfile))
                {
                    var arrFile = listfile.Split(',');
                    for (int j = 0; j < arrFile.Length; j++)
                    {
                        if (string.IsNullOrEmpty(arrFile[j].Trim())) continue;
                        try
                        {
                            arrFile[j] = arrFile[j].Trim().Replace("\\", "/");
                            if (!arrFile[j].StartsWith("/")) arrFile[j] = arrFile[j].Insert(0, "/");
                            arrFile[j] = arrFile[j].Replace(@" ", "%20");
                            arrFile[j] = arrFile[j].Replace(@"&", "%26");
                            files += (!string.IsNullOrEmpty(files) ? "," : "") + arrFile[j].Trim();
                        }
                        catch { }

                    }
                }
                else
                {
                    string[] fileEntries = System.IO.Directory.GetFiles(HttpContext.Current.Server.MapPath("~" + fileFolder));
                    if (fileEntries != null && fileEntries.Length > 0)
                    {
                        FileInfo fi = new FileInfo(fileEntries[0]);
                        string extn = fi.Extension;
                        int z = 1;
                        for (int j = 0; j < fileEntries.Length; j++)
                        {
                            FileInfo fd = new FileInfo(fileEntries[j]);
                            if (!fd.Name.Contains(model)) continue;
                            if (fd.Name.Contains(model + "(1)")) continue;
                            files += (!string.IsNullOrEmpty(files) ? "," : "") + (fileFolder + "/" + model + "(" + (z + 1) + ")" + extn);
                            files = files.Replace(@" ", "%20");
                            files = files.Replace(@"&", "%26");
                            z++;
                        }
                    }
                }
                try
                {
                    item.Code = Data.GetCode(item.Name + " " + item.Model);
                    item.Status = 18897;
                    item.Published = DateTime.Now;
                    item.Updated = DateTime.Now;
                    item.Order = GetMaxProductOrder();
                    item.CPUserID = CPLogin.UserID;
                    ModProductService.Instance.Save(item);
                    success++;
                    //update url
                    ModCleanURLService.Instance.InsertOrUpdate(item.Code, "Product", item.ID, item.MenuID, 1);
                    if (!string.IsNullOrEmpty(files))
                    {
                        ModProductFileService.Instance.InsertOrUpdate(item.ID, files.Split(','));
                    }
                }
                catch (Exception ex)
                {
                    error += (!string.IsNullOrEmpty(error) ? "," : "") + Name + "-" + ex.Message;
                    rowerror += (!string.IsNullOrEmpty(rowerror) ? "," : "") + (i + 1);
                    continue;
                }
            }

            fstream.Close();
            fstream.Dispose();
            return "Đã cập nhật thành công " + success + " sản phẩm";
        }
        private static int GetMaxProductOrder()
        {
            return ModProductService.Instance.CreateQuery()
                    .Max(o => o.Order)
                    .ToValue().ToInt(0) + 1;
        }
    }
}
