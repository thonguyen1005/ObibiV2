using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductClassifyDetailPriceEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int ClassifyDetailID1 { get; set; }

        [DataInfo]
        public int ClassifyDetailID2 { get; set; }
        [DataInfo]
        public long Price { get; set; }
        [DataInfo]
        public long Price2 { get; set; }
        [DataInfo]
        public int SoLuong { get; set; }
        [DataInfo]
        public string Sku { get; set; }

        #endregion Autogen by VSW

        private long _oSellOff;
        public long SellOff
        {
            get
            {
                if (_oSellOff == 0 && Price2 > Price)
                    _oSellOff = Price2 - Price;

                return _oSellOff;
            }
        }

        private long _oSellOffPercent;
        public long SellOffPercent
        {
            get
            {
                if (_oSellOffPercent == 0 && Price2 > 0 && SellOff > 0)
                    _oSellOffPercent = SellOff * 100 / Price2;

                return _oSellOffPercent;
            }
        }
    }

    public class ModProductClassifyDetailPriceService : ServiceBase<ModProductClassifyDetailPriceEntity>
    {
        #region Autogen by VSW

        public ModProductClassifyDetailPriceService() : base("[Mod_ProductClassifyDetailPrice]")
        {
        }

        private static ModProductClassifyDetailPriceService _instance;
        public static ModProductClassifyDetailPriceService Instance => _instance ?? (_instance = new ModProductClassifyDetailPriceService());

        #endregion Autogen by VSW

        public ModProductClassifyDetailPriceEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModProductClassifyDetailPriceEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }

        public ModProductClassifyDetailPriceEntity GetByProperty(int productId, int colorId, int sizeId)
        {
            return CreateQuery()
               .Where(o => o.ProductID == productId)
               .Where(o => o.ClassifyDetailID1 == colorId)
               .Where(o => o.ClassifyDetailID2 == sizeId)
               .ToSingle_Cache();
        }
    }
}