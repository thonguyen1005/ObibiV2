using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.Interface;

namespace VSW.Website.DataBase.Repositories
{
    public class SysSiteRepository : SqlRepository<SYS_SITEEntity, int>
    {
        public SysSiteRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }

        public SYS_SITEEntity GetByCode(string code)
        {
            return this.GetTable().Where(o=> o.Code == code).FirstOrDefault();
        }
        public SYS_SITEEntity GetById(int id)
        {
            return this.GetTable().Where(o => o.ID == id).FirstOrDefault();
        }
        public SYS_SITEEntity GetDefault()
        {
            return this.GetTable().Where(o => o.Default == true).FirstOrDefault();
        }
    }
}

