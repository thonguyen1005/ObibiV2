using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class ModOnlineEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public int WebUserID { get; set; }

        [DataInfo]
        public long TimeValue { get; set; }

        [DataInfo]
        public string SessionID { get; set; }

        [DataInfo]
        public string IP { get; set; }

        #endregion Autogen by VSW
    }

    public class ModOnlineService : ServiceBase<ModOnlineEntity>
    {
        #region Autogen by VSW

        public ModOnlineService() : base("[Mod_Online]")
        {
        }

        private static ModOnlineService _instance;
        public static ModOnlineService Instance => _instance ?? (_instance = new ModOnlineService());

        #endregion Autogen by VSW

        public ModOnlineEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}