using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;

namespace VSW.Website.DataBase.Repositories
{               
    public class CpAccessRepository : SqlRepository<CP_ACCESSEntity, string> 
    {
        public CpAccessRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }
    }
}

