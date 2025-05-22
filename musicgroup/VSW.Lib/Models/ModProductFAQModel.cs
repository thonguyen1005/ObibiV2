using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductFAQEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int FAQID { get; set; }

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

        private ModFAQEntity _oFAQ;

        public ModFAQEntity GetFAQ()
        {
            if (_oFAQ == null && FAQID > 0)
                _oFAQ = ModFAQService.Instance.GetByID_Cache(FAQID);

            return _oFAQ ?? (_oFAQ = new ModFAQEntity());
        }
    }

    public class ModProductFAQService : ServiceBase<ModProductFAQEntity>
    {
        #region Autogen by VSW

        public ModProductFAQService() : base("[Mod_ProductFAQ]")
        {
        }

        private static ModProductFAQService _instance;
        public static ModProductFAQService Instance => _instance ?? (_instance = new ModProductFAQService());

        #endregion Autogen by VSW

        public ModProductFAQEntity GetByID(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle();
        }

        public ModProductFAQEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id)
                .ToSingle_Cache();
        }

        public bool Exists(int productID, int FAQID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID && o.FAQID == FAQID)
                .Count()
                .ToValue_Cache()
                .ToBool();
        }

        public List<ModProductFAQEntity> GetAll_Cache(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList_Cache();
        }
        public List<ModProductFAQEntity> GetAll(int productID)
        {
            return CreateQuery()
                .Where(o => o.ProductID == productID)
                .ToList();
        }
    }
}