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

namespace VSW.Website.DataBase.Services
{
    public class SiteService : ISiteServiceInterface
    {
        private SysSiteRepository _repo;

        public SiteService(IWorkingContext<SiteService> context)
        {
            _repo = new SysSiteRepository(context: context);
        }

        public ISiteInterface VSW_Core_GetByCode(string code)
        {
            return _repo.GetByCode(code);
        }
        public ISiteInterface VSW_Core_GetByID(int id)
        {
            return _repo.GetById(id);
        }
        public ISiteInterface VSW_Core_GetDefault()
        {
            return _repo.GetDefault();
        }
    }
}
