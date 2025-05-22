using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModPropertyEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public int ProductID { get; set; }

        [DataInfo]
        public int PropertyID { get; set; }

        [DataInfo]
        public int PropertyValueID { get; set; }

        #endregion Autogen by VSW

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
                _oProduct = ModProductService.Instance.GetByID(ProductID);

            return _oProduct ?? (_oProduct = new ModProductEntity());
        }

        private WebPropertyEntity _oProperty;

        public WebPropertyEntity GetProperty()
        {
            if (_oProperty == null && PropertyID > 0)
                _oProperty = WebPropertyService.Instance.GetByID_Cache(PropertyID);

            return _oProperty ?? (_oProperty = new WebPropertyEntity());
        }

        private WebPropertyEntity _oPropertyValue;

        public WebPropertyEntity GetPropertyValue()
        {
            if (_oPropertyValue == null && PropertyValueID > 0)
                _oPropertyValue = WebPropertyService.Instance.GetByID_Cache(PropertyValueID);

            return _oPropertyValue ?? (_oPropertyValue = new WebPropertyEntity());
        }
    }

    public class ModPropertyService : ServiceBase<ModPropertyEntity>
    {
        #region Autogen by VSW

        private ModPropertyService() : base("[Mod_Property]")
        {
        }

        private static ModPropertyService _instance;
        public static ModPropertyService Instance => _instance ?? (_instance = new ModPropertyService());

        #endregion Autogen by VSW

        public ModPropertyEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public ModPropertyEntity GetByID(int productID, int menuID, int propertyID, int propertyValueID)
        {
            return CreateQuery()
               .Where(o => o.ProductID == productID && o.MenuID == menuID && o.PropertyID == propertyID && o.PropertyValueID == propertyValueID)
               .ToSingle();
        }

        public ModPropertyEntity GetByID_Cache(int productID, int menuID, int propertyID, int propertyValueID)
        {
            return CreateQuery()
               .Where(o => o.ProductID == productID && o.MenuID == menuID && o.PropertyID == propertyID && o.PropertyValueID == propertyValueID)
               .ToSingle_Cache();
        }

        public int GetCount(int propertyValueID)
        {
            return Instance.CreateQuery()
                        .Select(o => o.ID)
                        .Where(o => o.PropertyValueID == propertyValueID)
                        .Count()
                        .ToValue_Cache()
                        .ToInt(0);
        }
        public int GetCount(int propertyValueID, int menuID)
        {
            return Instance.CreateQuery()
                        .Select(o => o.ID)
                        .Where(o => o.PropertyValueID == propertyValueID && o.MenuID == menuID)
                        .Count()
                        .ToValue_Cache()
                        .ToInt(0);
        }
    }
}