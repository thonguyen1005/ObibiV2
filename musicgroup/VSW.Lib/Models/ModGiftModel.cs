using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModGiftEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public int MenuProductID { get; set; }

        [DataInfo]
        public int BrandID { get; set; }

        [DataInfo]
        public int State { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Model { get; set; }

        [DataInfo]
        public long Price { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public long View { get; set; }

        [DataInfo]
        public string Summary { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public string Custom { get; set; }

        [DataInfo]
        public string PageTitle { get; set; }

        [DataInfo]
        public string PageDescription { get; set; }

        [DataInfo]
        public string PageKeywords { get; set; }

        [DataInfo]
        public DateTime Published { get; set; }

        [DataInfo]
        public DateTime Updated { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        [DataInfo]
        public int ProductID { get; set; }
        [DataInfo]
        public string ProductCode { get; set; }
        public bool Check { get; set; }
        #endregion Autogen by VSW

        public ModGiftEntity()
        {
            Items.SetValue("IsName", true);
            Items.SetValue("IsSummary", true);
        }

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }
        private ModProductEntity _oProduct;
        public ModProductEntity GetProduct()
        {
            if (_oProduct == null && ProductID > 0)
                _oProduct = ModProductService.Instance.GetByID_Cache(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }
        public void UpView()
        {
            View++;
            ModGiftService.Instance.Save(this, o => o.View);
        }
    }

    public class ModGiftService : ServiceBase<ModGiftEntity>
    {
        #region Autogen by VSW

        public ModGiftService() : base("[Mod_Gift]")
        {
            DBExecuteMode = DBExecuteType.DataReader;
        }

        private static ModGiftService _instance;
        public static ModGiftService Instance => _instance ?? (_instance = new ModGiftService());

        #endregion Autogen by VSW

        public ModGiftEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public ModGiftEntity GetByID_Cache(int id)
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