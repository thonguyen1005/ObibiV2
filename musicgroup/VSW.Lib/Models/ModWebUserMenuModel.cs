using System;
using System.Collections.Generic;
using VSW.Core.Models;
using VSW.Lib.Global;

namespace VSW.Lib.Models
{
    public class ModWebUserMenuEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public int Count { get; set; }
        [DataInfo]
        public string Note { get; set; }
        [DataInfo]
        public DateTime Created { get; set; }
        [DataInfo]
        public int Order { get; set; }
        [DataInfo]
        public bool Activity { get; set; }
        #endregion Autogen by VSW
    }

    public class ModWebUserMenuService : ServiceBase<ModWebUserMenuEntity>
    {
        #region Autogen by VSW

        private ModWebUserMenuService() : base("[Mod_WebUserMenu]")
        {
        }

        private static ModWebUserMenuService _instance;
        public static ModWebUserMenuService Instance => _instance ?? (_instance = new ModWebUserMenuService());

        #endregion Autogen by VSW
        public ModWebUserMenuEntity GetByID(int id)
        {
            return CreateQuery()
                   .Where(o => o.ID == id)
                   .ToSingle();
        }
        public ModWebUserMenuEntity GetByID_Cache(int id)
        {
            return CreateQuery()
                   .Where(o => o.ID == id)
                   .ToSingle_Cache();
        }
    }
}