using System;
using System.Linq;
using System.Collections.Generic;
using VSW.Core.Services;
using VSW.Website.DataBase.Entities;

namespace VSW.Website.DataBase.Repositories
{
    public class ModProductClassifyDetailPriceRepository : SqlRepository<MOD_PRODUCTCLASSIFYDETAILPRICEEntity, int>
    {
        public ModProductClassifyDetailPriceRepository(SqlRepositoryOptions options = null, IWorkingContext context = null) : base(Constant.Datasource, options, context)
        {
        }

        public MOD_PRODUCTCLASSIFYDETAILPRICEEntity GetByProperty(int productId, int colorId, int sizeId)
        {
            return this.GetTable()
               .Where(o => o.ProductID == productId)
               .Where(o => o.ClassifyDetailID1 == colorId)
               .Where(o => o.ClassifyDetailID2 == sizeId)
               .FirstOrDefault();
        }
    }
}

