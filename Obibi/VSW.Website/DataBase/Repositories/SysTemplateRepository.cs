using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities; 

namespace VSW.Website.DataBase.Repositories
{               
    public class SysTemplateRepository : SqlRepository<SYS_TEMPLATEEntity, int> 
    {
        public SysTemplateRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }
        public SYS_TEMPLATEEntity GetByID(int id)
        {
            return this.Get(id);
        }
    }
}

