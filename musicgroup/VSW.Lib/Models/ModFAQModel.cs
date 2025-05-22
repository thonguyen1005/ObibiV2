using System;
using System.Collections.Generic;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModFAQEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int MenuID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Content { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }

        [DataInfo]
        public int Order { get; set; }

        [DataInfo]
        public bool Activity { get; set; }
        public bool Check { get; set; }

        #endregion

        private WebMenuEntity _oMenu;

        public WebMenuEntity GetMenu()
        {
            if (_oMenu == null && MenuID > 0)
                _oMenu = WebMenuService.Instance.GetByID_Cache(MenuID);

            return _oMenu ?? (_oMenu = new WebMenuEntity());
        }

    }

    public class ModFAQService : ServiceBase<ModFAQEntity>
    {

        #region Autogen by VSW

        private ModFAQService()
            : base("[Mod_FAQ]")
        {

        }

        private static ModFAQService _Instance = null;
        public static ModFAQService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModFAQService();

                return _Instance;
            }
        }

        #endregion

        public ModFAQEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModFAQEntity GetByID_Cache(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}