using System;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class CPUserLogEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int UserID { get; set; }

        [DataInfo]
        public string IP { get; set; }

        [DataInfo]
        public string Note { get; set; }

        [DataInfo]
        public DateTime Created { get; set; }

        #endregion Autogen by VSW

        private CPUserEntity _oUser;

        public CPUserEntity GetUser()
        {
            if (_oUser == null && UserID > 0)
                _oUser = CPUserService.Instance.GetByID(UserID);

            return _oUser ?? (_oUser = new CPUserEntity());
        }
    }

    public class CPUserLogService : ServiceBase<CPUserLogEntity>
    {
        #region Autogen by VSW

        public CPUserLogService() : base("[CP_UserLog]")
        {
        }

        private static CPUserLogService _instance;
        public static CPUserLogService Instance => _instance ?? (_instance = new CPUserLogService());

        #endregion Autogen by VSW
    }
}