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
    public class TemplateService : ITemplateServiceInterface
    {
        private SysTemplateRepository _repo;

        public TemplateService(IWorkingContext<TemplateService> context)
        {
            _repo = new SysTemplateRepository(context: context);
        }

        public ITemplateInterface VSW_Core_GetByID(int id)
        {
            return _repo.GetByID(id);
        }
    }
}
