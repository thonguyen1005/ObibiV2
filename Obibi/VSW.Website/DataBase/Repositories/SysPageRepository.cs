using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;
using VSW.Website.Interface;

namespace VSW.Website.DataBase.Repositories
{
    public class SysPageRepository : SqlRepository<SYS_PAGEEntity, int>
    {
        public SysPageRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }
        public SYS_PAGEEntity GetByID(int id)
        {
            return this.Get(id);
        }
    }
}

