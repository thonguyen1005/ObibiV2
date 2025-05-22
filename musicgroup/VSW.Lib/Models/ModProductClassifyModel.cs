using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductClassifyEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        #endregion Autogen by VSW

    }

    public class ModProductClassifyService : ServiceBase<ModProductClassifyEntity>
    {
        #region Autogen by VSW

        public ModProductClassifyService() : base("[Mod_ProductClassify]")
        {
        }

        private static ModProductClassifyService _instance;
        public static ModProductClassifyService Instance => _instance ?? (_instance = new ModProductClassifyService());

        #endregion Autogen by VSW

        public ModProductClassifyEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModProductClassifyEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}