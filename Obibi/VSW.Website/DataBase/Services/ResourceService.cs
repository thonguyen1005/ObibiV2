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
    public class ResourceService : IResourceServiceInterface
    {
        private WebResourceRepository _repo;
        private ILocalCache _cache;

        public ResourceService(ILocalCache cache, IWorkingContext<ResourceService> context)
        {
            _repo = new WebResourceRepository(context: context);
            _cache = cache;
        }

        public async Task<string> ParseAsync(string key, HttpContext context)
        {
            string value = "";
            if (_cache.HasKey("RS:" + key))
            {
                value = _cache.Get<string>("RS:" + key);
                return value;
            }
            else
            {
                int langId = 1;
                if (context.Items["Site"] is not null)
                {
                    var site = context.Items["Site"] as SYS_SITEEntity;
                    if (site != null)
                    {
                        langId = site.LangID;
                    }
                }
                value = await _repo.GetTable().Where(o => o.Code == key && o.LangID == langId).Select(o => o.Value).FirstOrDefaultAsync();
                if (value.IsNotEmpty())
                {
                    _cache.Set("RS:" + key, value);
                }
            }
            return value;
        }
    }
}
