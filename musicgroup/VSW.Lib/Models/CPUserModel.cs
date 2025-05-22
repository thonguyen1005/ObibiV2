using System.Collections.Generic;
using VSW.Core.Models;
using VSW.Lib.Global;

namespace VSW.Lib.Models
{
    public class CPUserEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public string LoginName { get; set; }

        [DataInfo]
        public string Password { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public string Address { get; set; }

        [DataInfo]
        public string Phone { get; set; }

        [DataInfo]
        public string Email { get; set; }

        [DataInfo]
        public bool Activity { get; set; }

        #endregion Autogen by VSW

        private List<CPRoleEntity> _getRole;

        public List<CPRoleEntity> GetRole()
        {
            if (_getRole == null)
                _getRole = CPRoleService.Instance.GetByUserID(ID);

            return _getRole ?? (_getRole = new List<CPRoleEntity>());
        }

        public bool HasRole(string roleCode)
        {
            return GetRole().Find(o => o.Code == roleCode) != null;
        }

        public bool HasRoleAdministrator()
        {
            return HasRole("Administrator");
        }

        public bool IsCPDesign => IsAdministrator;

        public bool IsAdministrator => GetPermissionsByModule("SysAdministrator").Any;

        private Dictionary<string, Permissions> _dicModulePermissions;

        public Permissions GetPermissionsByModule(string moduleCode)
        {
            if (_dicModulePermissions == null)
                _dicModulePermissions = new Dictionary<string, Permissions>();

            if (_dicModulePermissions.ContainsKey(moduleCode))
                return _dicModulePermissions[moduleCode];

            return GetPermissionsByRef("CP.MODULE", moduleCode);
        }

        private Dictionary<string, Permissions> _dicRefPermissions;

        public Permissions GetPermissionsByRef(string type, string moduleCode)
        {
            if (_dicRefPermissions == null)
                _dicRefPermissions = new Dictionary<string, Permissions>();

            var cacheKey = type + "." + moduleCode;

            if (_dicRefPermissions.ContainsKey(cacheKey))
                return _dicRefPermissions[cacheKey];

            var permissions = new Permissions();

            var access = CPAccessService.Instance.GetByUser(type, moduleCode, ID);

            if (access != null)
                permissions = new Permissions(access.Value);

            if (permissions.Full)
            {
                _dicRefPermissions[cacheKey] = permissions;

                return permissions;
            }

            var getRole = GetRole();
            for (var i = 0; getRole != null && i < getRole.Count; i++)
            {
                access = CPAccessService.Instance.GetByRole(type, moduleCode, getRole[i].ID);

                if (access != null)
                    permissions.OrAccess(access.Value);

                if (permissions.Full)
                    break;
            }

            _dicRefPermissions[cacheKey] = permissions;

            return permissions;
        }
    }

    public class CPUserService : ServiceBase<CPUserEntity>
    {
        #region Autogen by VSW

        public CPUserService() : base("[CP_User]")
        {
        }

        private static CPUserService _instance;

        public static CPUserService Instance => _instance ?? (_instance = new CPUserService());

        #endregion Autogen by VSW

        public CPUserEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public CPUserEntity GetByEmail(string email)
        {
            return CreateQuery()
                .Where(o => o.Email == email)
                .ToSingle();
        }

        public CPUserEntity GetLogin(int id)
        {
            return CreateQuery()
                .Where(o => o.ID == id && o.Activity == true)
                .ToSingle();
        }

        public CPUserEntity GetLogin(string loginName, string password)
        {
            return GetLoginMd5(loginName, Security.Md5(password));
        }

        public CPUserEntity GetLoginMd5(string loginName, string password)
        {
            return CreateQuery()
                .Where(o => o.LoginName == loginName && o.Password == password && o.Activity == true)
                .ToSingle();
        }
    }
}