using System;
using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductTagEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public int TagID { get; set; }
        [DataInfo]
        public int ProductID { get; set; }

        #endregion      
  
        private ModTagEntity _oTag = null;
        public ModTagEntity getTag()
        {
            if (_oTag == null && TagID > 0)
                _oTag = ModTagService.Instance.GetByID(TagID);

            if (_oTag == null)
                _oTag = new ModTagEntity();

            return _oTag;
        }      
  
        private ModProductEntity _oProduct = null;
        public ModProductEntity getProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID(ProductID);

            if (_oProduct == null)
                _oProduct = new ModProductEntity();

            return _oProduct;
        }

    }

    public class ModProductTagService : ServiceBase<ModProductTagEntity>
    {

        #region Autogen by VSW

        private ModProductTagService()
            : base("[Mod_ProductTag]")
        {

        }

        private static ModProductTagService _Instance = null;
        public static ModProductTagService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModProductTagService();

                return _Instance;
            }
        }

        #endregion

        public ModProductTagEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

    }
}