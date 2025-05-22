using System.Collections.Generic;
using VSW.Core.Global;
using VSW.Core.Models;
using VSW.Core.Web;

namespace VSW.Lib.Models
{
    public class WebPropertyConfigEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public int PropertyID { get; set; }

        [DataInfo]
        public int PropertyParentID { get; set; }

        [DataInfo]
        public int BrandID { get; set; }

        [DataInfo]
        public bool IsShowMenu { get; set; }

        [DataInfo]
        public bool ShowFilterFast { get; set; }

        [DataInfo]
        public bool ShowBreadCrumb { get; set; }
        #endregion Autogen by VSW

        private WebPropertyEntity _oProperty;

        public WebPropertyEntity getProperty
        {
            get
            {
                if (_oProperty == null)
                    _oProperty = WebPropertyService.Instance.GetByID_Cache(PropertyID);

                return _oProperty ?? (_oProperty = new WebPropertyEntity());
            }
        }
        private WebMenuEntity _oMenu;

        public WebMenuEntity getMenu
        {
            get
            {
                if (_oMenu == null)
                    _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

                return _oMenu ?? (_oMenu = new WebMenuEntity());
            }
        }
        private WebMenuEntity _oBrand;

        public WebMenuEntity getBrand
        {
            get
            {
                if (_oBrand == null)
                    _oBrand = WebMenuService.Instance.GetByID_Cache(BrandID);

                return _oBrand ?? (_oBrand = new WebMenuEntity());
            }
        }
    }

    public class WebPropertyConfigService : ServiceBase<WebPropertyConfigEntity>
    {
        #region Autogen by VSW

        public WebPropertyConfigService() : base("[Web_PropertyConfig]")
        {
        }

        private static WebPropertyConfigService _instance;
        public static WebPropertyConfigService Instance => _instance ?? (_instance = new WebPropertyConfigService());

        #endregion Autogen by VSW

        public WebPropertyConfigEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public WebPropertyConfigEntity GetByID_Cache(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}