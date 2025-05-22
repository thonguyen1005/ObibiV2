using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities; 

namespace VSW.Website.DataBase.Repositories
{               
    public class ModProductClassifyDetailRepository : SqlRepository<MOD_PRODUCTCLASSIFYDETAILEntity, int> 
    {
        public ModProductClassifyDetailRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }
    }
}

