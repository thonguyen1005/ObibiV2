using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class CPUserRoleEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public int UserID { get; set; }

        [DataInfo]
        public int RoleID { get; set; }

        #endregion Autogen by VSW
    }

    public class CPUserRoleService : ServiceBase<CPUserRoleEntity>
    {
        #region Autogen by VSW

        public CPUserRoleService() : base("[CP_UserRole]")
        {
        }

        private static CPUserRoleService _instance;

        public static CPUserRoleService Instance => _instance ?? (_instance = new CPUserRoleService());

        #endregion Autogen by VSW
    }
}