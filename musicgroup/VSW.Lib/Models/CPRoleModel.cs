using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class CPRoleEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public bool Lock { get; set; }

        [DataInfo]
        public int Order { get; set; }

        #endregion Autogen by VSW
    }

    public class CPRoleService : ServiceBase<CPRoleEntity>
    {
        #region Autogen by VSW

        public CPRoleService() : base("[CP_Role]")
        {
        }

        private static CPRoleService _instance;
        public static CPRoleService Instance => _instance ?? (_instance = new CPRoleService());

        #endregion Autogen by VSW

        public CPRoleEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public List<CPRoleEntity> GetByUserID(int userID)
        {
            return CreateQuery()
                .WhereIn(o => o.ID, CPUserRoleService.Instance.CreateQuery()
                                                      .Select(o => o.RoleID)
                                                      .Where(o => o.UserID == userID)
                        )
                .ToList();
        }
    }
}