using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModNewsProductEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int NewsID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW

        private ModProductEntity _oProduct;

        public ModProductEntity GetProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID_Cache(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }

        private ModNewsEntity _oNews;

        public ModNewsEntity GetNews()
        {
            if (_oNews == null && NewsID > 0)
                _oNews = ModNewsService.Instance.GetByID_Cache(NewsID);

            return _oNews ?? (_oNews = new ModNewsEntity());
        }
    }

    public class ModNewsProductService : ServiceBase<ModNewsProductEntity>
    {
        #region Autogen by VSW

        public ModNewsProductService() : base("[Mod_NewsProduct]")
        {
        }

        private static ModNewsProductService _instance;
        public static ModNewsProductService Instance => _instance ?? (_instance = new ModNewsProductService());

        #endregion Autogen by VSW

        public ModNewsProductEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModNewsProductEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int newsID, int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.NewsID == newsID)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }
    }
}