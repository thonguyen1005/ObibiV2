using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using VSW.Core;
using VSW.Website.DataBase.Entities;
using VSW.Website.DataBase.Models;
using VSW.Website.Models;

namespace VSW.Website
{
    class MapperConfig : IMapperConfig
    {
        public void Register()
        {
            this.Register<MOD_NEWSEntity, ModNewsModel>();
            this.Register<MOD_PRODUCTEntity, ModProductModel>();
            this.Register<SYS_PAGEEntity, SysPageModel>();
        }
    }
}
