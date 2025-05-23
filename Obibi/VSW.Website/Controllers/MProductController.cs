using LinqToDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;
using VSW.Core;
using VSW.Core.Services;
using VSW.Core.Utils;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.DataBase.Repositories;
using VSW.Website.Helpers;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.Controllers
{
    public class MProductController : BaseController
    {
        public int MenuID { get; set; }
        public int BrandID { get; set; }
        public int State { get; set; }
        public int PageSize { get; set; } = 20;

        private ModProductRepository _repo = null;
        private SysPageRepository _repoPage = null;
        private ModPropertyRepository _repoProperty = null;
        private WebMenuRepository _repoMenu = null;
        public MProductController(IWorkingContext<MProductController> context) : base(context)
        {
            _repo = new ModProductRepository(context: context);
            _repoPage = new SysPageRepository(context: context);
            _repoProperty = new ModPropertyRepository(context: context);
            _repoMenu = new WebMenuRepository(context: context);
        }
        public async Task<IActionResult> Index(ProductSearchModel searchModel)
        {
            var dbQuery = _repo.GetTable()
                            .Select(o => new ModProductModel
                            {
                                ID = o.ID,
                                Name = o.Name,
                                PhienBan = o.PhienBan,
                                MenuID = o.MenuID,
                                BrandID = o.BrandID,
                                State = o.State,
                                Code = o.Code,
                                Model = o.Model,
                                File = o.File,
                                Price = o.Price,
                                Price2 = o.Price2,
                                View = o.View,
                                Published = o.Published,
                                Order = o.Order,
                                Activity = o.Activity
                            })
                            .Where(o => o.Activity == true)
                            .Where(State > 0, o => (o.State & State) == State);

            var lstMenuId = new List<int>();
            if (MenuID > 0)
            {
                lstMenuId = _repoMenu.GetChildIDForWeb_Cache("Product", MenuID, CurrentSite.LangID);
                if (lstMenuId.IsNotEmpty())
                {
                    dbQuery = dbQuery.Where(o => lstMenuId.Contains(o.MenuID));
                }
            }
            if (searchModel.b > 0)
            {
                dbQuery = dbQuery.Where(o => o.BrandID == searchModel.b);
            }
            else
            {
                dbQuery = dbQuery.Where(BrandID > 0, o => o.BrandID == BrandID);
            }

            string sort = searchModel.s;
            if (sort == "new_asc") dbQuery = dbQuery.OrderByDescending(o => o.Published);
            else if (sort == "price_asc") dbQuery = dbQuery.OrderBy(o => o.Price);
            else if (sort == "price_desc") dbQuery = dbQuery.OrderByDescending(o => o.Price);
            else if (sort == "view_desc") dbQuery = dbQuery.OrderByDescending(o => o.View);
            else dbQuery.OrderBy(o => new { o.Order, o.ID });

            string atr = searchModel.p;
            if (atr.IsNotEmpty() && lstMenuId.IsNotEmpty())
            {
                var lstArrId = atr.Split(',').Select(o => o.ToInt()).ToArray().ToList();
                var lstPropertyId = await _repoProperty.GetTable().Where(o => lstMenuId.Contains(o.MenuID)).Where(o => lstArrId.Contains(o.PropertyValueID)).Select(o => o.ProductID).Distinct().ToListAsync();
                if (lstPropertyId.IsNotEmpty())
                {
                    dbQuery = dbQuery.Where(o => lstPropertyId.Contains(o.ID));
                }
            }

            var model = await dbQuery.ToPagingAsync<ModProductModel>(searchModel.Page, searchModel.PageSize);

            searchModel.TotalRecord = model.TotalCount;
            ViewBag.Model = searchModel;
            if (lstMenuId.IsNotEmpty())
            {
                string sqlProperty = @"SELECT DISTINCT mp.MenuID, mp.ProductID, mp.PropertyID, mp.PropertyValueID, wp.Code as PropertyCode, wp.Name as PropertyName, wp2.Code as PropertyValueCode, wp2.Name as PropertyValueName, wp2.File
                                    FROM Mod_Property mp
                                    JOIN Web_Property wp ON wp.ID = mp.PropertyID
                                    JOIN Web_Property wp2 ON wp2.ID = mp.PropertyValueID
                                    JOIN Mod_Product p ON mp.ProductID = p.ID";
                sqlProperty += " Where p.MenuID in (" + string.Join(",", lstMenuId) + @")";
                ViewBag.Propertys = await _repoProperty.WithSqlText(sqlProperty).QueryAsync<ModPropertyModel>();

                string sqlBrand = @"SELECT *
                                    FROM Web_Menu m ";
                sqlBrand += " Where m.ID in (select p.BrandID from Mod_Product p where p.MenuID in (" + string.Join(",", lstMenuId) + @") group by p.BrandID)";
                ViewBag.Brands = await _repoProperty.WithSqlText(sqlBrand).QueryAsync<WEB_MENUEntity>();
            }

            var lstPage = await _repoPage.GetTable()
                                        .Where(o => o.LangID == CurrentSite.LangID && o.Activity == true)
                                        .Where(o => o.ParentID == CurrentPage.ID)
                                        .OrderBy(o => new { o.Order, o.ID })
                                        .ToListAsync();
            if (lstPage.IsEmpty() && CurrentPage.ParentID > 1)
            {
                lstPage = await _repoPage.GetTable()
                                        .Where(o => o.LangID == CurrentSite.LangID && o.Activity == true)
                                        .Where(o => o.ParentID == CurrentPage.ParentID)
                                        .OrderBy(o => new { o.Order, o.ID })
                                        .ToListAsync();
            }

            ViewBag.Pages = lstPage;

            return View(model);
        }
        public async Task<IActionResult> Detail([FromRoute] string id)
        {
            if (CurrentPage.MenuID > 0)
                MenuID = CurrentPage.MenuID;

            var item = await _repo.GetTable()
                                 .Where(o => o.Activity == true && o.Code == id)
                                 .SingleAsync();

            var model = item.MapTo<ModProductModel>();
            if (item != null)
            {
                //up view
                item.View++;
                await _repo.UpdateColumnAsync(o => o.ID == item.ID, o => new MOD_PRODUCTEntity
                {
                    View = item.View,
                });

                var dbQuery = _repo.GetTable()
                           .Select(o => new ModProductModel
                           {
                               ID = o.ID,
                               Name = o.Name,
                               PhienBan = o.PhienBan,
                               MenuID = o.MenuID,
                               BrandID = o.BrandID,
                               State = o.State,
                               Code = o.Code,
                               Model = o.Model,
                               File = o.File,
                               Price = o.Price,
                               Price2 = o.Price2,
                               View = o.View,
                               Published = o.Published,
                               Order = o.Order,
                               Activity = o.Activity
                           }).Where(o => o.ID != item.ID)
                           .Where(o => o.Activity == true);

                var lstMenuId = new List<int>();
                if (MenuID > 0)
                {
                    lstMenuId = _repoMenu.GetChildIDForWeb_Cache("Product", MenuID, CurrentSite.LangID);
                    if (lstMenuId.IsNotEmpty())
                    {
                        dbQuery = dbQuery.Where(o => lstMenuId.Contains(o.MenuID));
                    }
                }

                ViewBag.Other = await dbQuery.OrderBy(o => new { o.Price, o.Order, o.ID })
                                .Take(PageSize)
                                .ToListAsync();

                ViewData["Title"] = string.IsNullOrEmpty(item.PageTitle) ? item.Name : item.PageTitle;
                ViewData["Keyword"] = !string.IsNullOrEmpty(item.PageKeywords) ? item.PageKeywords : CurrentPage.PageKeywords;
                ViewData["Description"] = string.IsNullOrEmpty(item.PageDescription) ? item.Summary : item.PageDescription;

                ViewData["Image"] = Request.Scheme + "://" + Request.Host + ImageHelper.ResizeToWebp(item.File);
                ViewData["Image_Url"] = Request.Scheme + "://" + Request.Host + ImageHelper.ResizeToWebp(item.File);
            }
            ViewBag.Layout = "Detail";
            return View();
        }
    }
}
