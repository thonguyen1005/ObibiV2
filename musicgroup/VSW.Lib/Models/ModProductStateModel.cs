using System;
using System.Collections.Generic;

using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModProductStateEntity : EntityBase
    {
        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public int Value { get; set; }
    }

    public class ModProductStateService : ServiceBase<ModProductStateEntity>
    {

        #region Autogen by VSW

        private ModProductStateService()
            : base("[Mod_ProductState]")
        {

        }

        private static ModProductStateService _Instance = null;
        public static ModProductStateService Instance
        {
            get
            {
                if (_Instance == null)
                    _Instance = new ModProductStateService();

                return _Instance;
            }
        }

        #endregion

        public ModProductStateEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
        public ModProductStateEntity GetByID_Cache(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }
    }
}