using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductClassifyDetailEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int ClassifyID { get; set; }

        [DataInfo]
        public string File { get; set; }

        #endregion Autogen by VSW
        private ModProductClassifyEntity _oParent;
        public ModProductClassifyEntity GetParent()
        {
            if (_oParent == null && ClassifyID > 0)
                _oParent = ModProductClassifyService.Instance.GetByID_Cache(ClassifyID);

            return _oParent ?? (_oParent = new ModProductClassifyEntity());
        }
    }

    public class ModProductClassifyDetailService : ServiceBase<ModProductClassifyDetailEntity>
    {
        #region Autogen by VSW

        public ModProductClassifyDetailService() : base("[Mod_ProductClassifyDetail]")
        {
        }

        private static ModProductClassifyDetailService _instance;
        public static ModProductClassifyDetailService Instance => _instance ?? (_instance = new ModProductClassifyDetailService());

        #endregion Autogen by VSW

        public ModProductClassifyDetailEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModProductClassifyDetailEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}