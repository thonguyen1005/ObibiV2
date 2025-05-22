using VSW.Core.Interface;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class SysLangEntity : EntityBase, ILangInterface
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        #endregion Autogen by VSW
    }

    public class SysLangService : ServiceBase<SysLangEntity>, ILangServiceInterface
    {
        #region Autogen by VSW

        public SysLangService() : base("[Sys_Lang]")
        {
        }

        private static SysLangService _instance;

        public static SysLangService Instance => _instance ?? (_instance = new SysLangService());

        #endregion Autogen by VSW

        public SysLangEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        #region ILangServiceInterface Members

        public ILangInterface VSW_Core_GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }

        #endregion ILangServiceInterface Members
    }
}