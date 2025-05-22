using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities; 

namespace VSW.Website.DataBase.Repositories
{               
    public class ModWebuserFileRepository : SqlRepository<MOD_WEBUSERFILEEntity, int> 
    {
        public ModWebuserFileRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }
    }
}

