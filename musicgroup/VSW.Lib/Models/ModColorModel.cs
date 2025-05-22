using System;
using System.Collections.Generic;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModColorEntity :EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string ColorCode { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        public int ProductSizeID { get; set; }
        #endregion

        private WebMenuEntity _oMenu = null;
        public WebMenuEntity getMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            if (_oMenu == null)
                _oMenu = new WebMenuEntity();

            return _oMenu;
        }
    }

    public class ModColorService : ServiceBase<ModColorEntity>
    {
        #region Autogen by VSW

        public ModColorService()
            : base("[Mod_Color]")
        {

        }

        private static ModColorService _Instance = null;
        public static ModColorService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModColorService();

                return _Instance;
            }
        }

        #endregion

        public ModColorEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModColorEntity GetByID_Cache(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}
