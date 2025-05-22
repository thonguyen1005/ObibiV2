using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class CPAccessEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public string RefCode { get; set; }

        [DataInfo]
        public int RoleID { get; set; }

        [DataInfo]
        public int UserID { get; set; }

        [DataInfo]
        public string Type { get; set; }

        [DataInfo]
        public int Value { get; set; }

        #endregion Autogen by VSW
    }

    public class CPAccessService : ServiceBase<CPAccessEntity>
    {
        #region Autogen by VSW

        public CPAccessService() : base("[CP_Access]")
        {
        }

        private static CPAccessService _instance;

        public static CPAccessService Instance => _instance ?? (_instance = new CPAccessService());

        #endregion Autogen by VSW

        public CPAccessEntity GetByUser(string type, string refCode, int userId)
        {
            return CreateQuery()
                .Where(o => o.UserID == userId && o.RefCode == refCode && o.Type == type)
                .ToSingle();
        }

        public CPAccessEntity GetByRole(string type, string refCode, int roleId)
        {
            return CreateQuery()
                .Where(o => o.RoleID == roleId && o.RefCode == refCode && o.Type == type)
                .ToSingle();
        }
    }
}