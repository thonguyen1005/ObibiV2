using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductVideoEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int VideoID { get; set; }

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

        private ModVideoEntity _oVideo;

        public ModVideoEntity GetVideo()
        {
            if (_oVideo == null && VideoID > 0)
                _oVideo = ModVideoService.Instance.GetByID_Cache(VideoID);

            return _oVideo ?? (_oVideo = new ModVideoEntity());
        }
    }

    public class ModProductVideoService : ServiceBase<ModProductVideoEntity>
    {
        #region Autogen by VSW

        public ModProductVideoService() : base("[Mod_ProductVideo]")
        {
        }

        private static ModProductVideoService _instance;
        public static ModProductVideoService Instance => _instance ?? (_instance = new ModProductVideoService());

        #endregion Autogen by VSW

        public ModProductVideoEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModProductVideoEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int productID, int VideoID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.VideoID == VideoID)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

        public List<ModProductVideoEntity> GetAll_Cache(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList_Cache();
        }
        public List<ModProductVideoEntity> GetAll(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList();
        }
    }
}