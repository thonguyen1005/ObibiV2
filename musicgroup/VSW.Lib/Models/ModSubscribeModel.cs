using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModSubscribeEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }
        [DataInfo]
        public override string Name { get; set; }
        [DataInfo]
        public string Email { get; set; }
        [DataInfo]
        public string Phone { get; set; }
        [DataInfo]
        public string IP { get; set; }
        [DataInfo]
        public DateTime Created { get; set; }

        #endregion
    }

    public class ModSubscribeService : ServiceBase<ModSubscribeEntity>
    {
        #region Autogen by VSW

        public ModSubscribeService()
            : base("[Mod_Subscribe]")
        {

        }

        private static ModSubscribeService _instance;
        public static ModSubscribeService Instance
        {
            get { return _instance ?? (_instance = new ModSubscribeService()); }
        }

        #endregion

        public ModSubscribeEntity GetByID(int id)
        {
            return base.CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}
