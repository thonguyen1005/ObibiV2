using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModPromotionProductEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public int PromotionID { get; set; }

        #endregion Autogen by VSW

        private ModProductEntity _oProduct;

        public ModProductEntity GetProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID_Cache(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }

        private ModPromotionEntity _oPromotion;

        public ModPromotionEntity GetPromotion()
        {
            if (_oPromotion == null && PromotionID > 0)
                _oPromotion = ModPromotionService.Instance.GetByID_Cache(PromotionID);

            return _oPromotion ?? (_oPromotion = new ModPromotionEntity());
        }
    }

    public class ModPromotionProductService : ServiceBase<ModPromotionProductEntity>
    {
        #region Autogen by VSW

        public ModPromotionProductService() : base("[Mod_PromotionProduct]")
        {
        }

        private static ModPromotionProductService _instance;
        public static ModPromotionProductService Instance => _instance ?? (_instance = new ModPromotionProductService());

        #endregion Autogen by VSW

        public ModPromotionProductEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModPromotionProductEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int promotionID, int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.PromotionID == promotionID)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

        public List<ModPromotionProductEntity> GetAll_Cache(int promotionID)
        {
            return CreateQuery()
                .Where(o => o.PromotionID == promotionID)
                .ToList_Cache();
        }
        public List<ModPromotionProductEntity> GetAll(int promotionID)
        {
            return CreateQuery()
                .Where(o => o.PromotionID == promotionID)
                .ToList();
        }
    }
}