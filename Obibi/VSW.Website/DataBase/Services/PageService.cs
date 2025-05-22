using System.Text.RegularExpressions;
using System;
using VSW.Website.DataBase.Repositories;
using VSW.Core.Services;
using VSW.Core.Caching;
using LinqToDB;
using VSW.Core;
using VSW.Website.DataBase.Entities;
using Microsoft.Extensions.FileSystemGlobbing;
using VSW.Website.Interface;
using VSW.Website.Models;

namespace VSW.Website.DataBase.Services
{
    public class PageService : IPageServiceInterface
    {
        private SysPageRepository _repo;
        private ModCleanurlRepository _repoCleanurl;
        private WebMenuRepository _repoMenu;

        public PageService(IWorkingContext<PageService> context)
        {
            _repo = new SysPageRepository(context: context);
            _repoCleanurl = new ModCleanurlRepository(context: context);
            _repoMenu = new WebMenuRepository(context: context);
        }

        public Tuple<IPageInterface, string> VSW_Core_GetByID(int id)
        {
            var page = _repo.GetByID(id);
            return new Tuple<IPageInterface, string>(page, "/" + page.ModuleCode + "/Index");
        }
        public Tuple<IPageInterface, string> VSW_Core_CurrentPage(VQS vqs, int langId)
        {
            if(vqs == null) return new Tuple<IPageInterface, string>(null, "/Home/Error");
            IPageInterface page = null;
            if (vqs.Count > 1)
            {
                page = _repo.GetTable().Where(o => o.Code == vqs.BeginCode && o.Activity == true && o.LangID == langId).FirstOrDefault();
                if(page != null) return new Tuple<IPageInterface, string>(page, "/" + page.ModuleCode + "/Detail/" + vqs.EndCode);
            }

            string code = vqs.EndCode;
            var cleanUrl = _repoCleanurl.GetTable().Where(o => o.Code == code && o.LangID == langId).FirstOrDefault();
            
            if (cleanUrl != null)
            {
                if (cleanUrl.Type == "Page")
                {
                    page = _repo.GetByID(cleanUrl.Value);
                    return new Tuple<IPageInterface, string>(page, "/" + page.ModuleCode + "/Index");
                }

                var menuId = cleanUrl.MenuID;
                while (menuId > 0)
                {
                    var id = menuId;
                    page = _repo.GetTable().Where(o => o.MenuID == id && o.BrandID < 1 && o.Activity == true).FirstOrDefault();
                    if (page != null) break;

                    var menu = _repoMenu.Get(menuId);
                    if (menu == null || menu.ParentID == 0) break;

                    menuId = menu.ParentID;
                }
            }
            if (page == null)
            {
                return new Tuple<IPageInterface, string>(page, "/Home/Error");
            }
            return new Tuple<IPageInterface, string>(page, "/" + page.ModuleCode + "/Detail/" + code);
        }

        public string GetUrlByModule(string moduleCode, int langId)
        {
            return _repo.GetTable().Where(o => o.LangID == langId && o.ModuleCode == moduleCode).Select(o => o.Code).FirstOrDefaultWithCache();
        }
    }
}
