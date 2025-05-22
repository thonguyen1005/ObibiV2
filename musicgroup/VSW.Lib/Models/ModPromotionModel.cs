using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModPromotionEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }
        [DataInfo]
        public int BrandID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public DateTime FromDate { get; set; }

        [DataInfo]
        public DateTime ToDate { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }
        
        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }

        private WebMenuEntity _oBrand;

        public WebMenuEntity GetBrand()
        {
            if (_oBrand == null && BrandID > 0)
                _oBrand = WebMenuService.Instance.GetByID_Cache(BrandID);

            return _oBrand ?? (_oBrand = new WebMenuEntity());
        }

        private List<ModProductEntity> _oGetProduct;
        public List<ModProductEntity> GetProduct()
        {
            _oGetProduct = new List<ModProductEntity>();
            if ((_oGetProduct == null || _oGetProduct.Count < 1) && ID > 0)
            {
                var listItem = ModPromotionProductService.Instance.CreateQuery()
                                                .Select(o => o.ProductID)
                                                .Where(o => o.PromotionID == ID)
                                                .ToList_Cache();

                for (int i = 0; listItem != null && i < listItem.Count; i++)
                {
                    var item = ModProductService.Instance.GetDataSelectByID_Cache(listItem[i].ProductID);
                    if (item != null)
                        _oGetProduct.Add(item);
                }
            }

            return _oGetProduct;
        }
    }

    public class ModPromotionService : ServiceBase<ModPromotionEntity>
    {
        #region Autogen by VSW

        public ModPromotionService() : base("[Mod_Promotion]")
        {
        }

        private static ModPromotionService _instance;
        public static ModPromotionService Instance => _instance ?? (_instance = new ModPromotionService());

        #endregion Autogen by VSW

        public ModPromotionEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModPromotionEntity GetByID_Cache(int id)
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