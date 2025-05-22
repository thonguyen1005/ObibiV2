using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductGiftEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int GiftID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public long Price { get; set; }

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

        private ModGiftEntity _oGift;

        public ModGiftEntity GetGift()
        {
            if (_oGift == null && GiftID > 0)
                _oGift = ModGiftService.Instance.GetByID_Cache(GiftID);

            return _oGift ?? (_oGift = new ModGiftEntity());
        }
    }

    public class ModProductGiftService : ServiceBase<ModProductGiftEntity>
    {
        #region Autogen by VSW

        public ModProductGiftService() : base("[Mod_ProductGift]")
        {
        }

        private static ModProductGiftService _instance;
        public static ModProductGiftService Instance => _instance ?? (_instance = new ModProductGiftService());

        #endregion Autogen by VSW

        public ModProductGiftEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModProductGiftEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int productID, int giftID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.GiftID == giftID)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

        public List<ModProductGiftEntity> GetAll_Cache(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList_Cache();
        }
        public List<ModProductGiftEntity> GetAll(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList();
        }
    }
}