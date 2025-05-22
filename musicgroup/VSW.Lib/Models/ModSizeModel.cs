using System;
using System.Collections.Generic;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModSizeEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

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

    public class ModSizeService : ServiceBase<ModSizeEntity>
    {
        #region Autogen by VSW

        public ModSizeService()
            : base("[Mod_Size]")
        {

        }

        private static ModSizeService _Instance = null;
        public static ModSizeService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModSizeService();

                return _Instance;
            }
        }

        #endregion

        public ModSizeEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModSizeEntity GetByID_Cache(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}
