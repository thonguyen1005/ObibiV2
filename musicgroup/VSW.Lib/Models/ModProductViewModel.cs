using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductViewEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public long View { get; set; }
        [DataInfo]
        public string WebUserCode { get; set; }

        #endregion Autogen by VSW

        private ModProductEntity _oProduct;
        public ModProductEntity GetProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }
    }

    public class ModProductViewService : ServiceBase<ModProductViewEntity>
    {
        #region Autogen by VSW

        public ModProductViewService() : base("[Mod_ProductView]")
        {
            DBConfigKey = "DBConnectionAffiliate";
        }

        private static ModProductViewService _instance;

        public static ModProductViewService Instance => _instance ?? (_instance = new ModProductViewService());

        #endregion Autogen by VSW

        public ModProductViewEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModProductViewEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }

        public bool Exists(string query)
        {
            return CreateQuery()
                           .Where(query)
                           .Count()
                           .ToValue()
                           .ToBool();
        }
    }
}