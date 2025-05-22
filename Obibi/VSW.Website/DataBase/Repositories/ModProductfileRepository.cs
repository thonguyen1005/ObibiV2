using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities; 

namespace VSW.Website.DataBase.Repositories
{               
    public class ModProductfileRepository : SqlRepository<MOD_PRODUCTFILEEntity, int> 
    {
        public ModProductfileRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }
    }
}

