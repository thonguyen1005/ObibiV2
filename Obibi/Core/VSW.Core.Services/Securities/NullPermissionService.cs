using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public class NullPermissionService : IPermissionService
    {
        public bool IsGrant(int staffId, string permission)
        {
            return true;
        }

        public bool IsGrant(int staffId, string permission, string app_code)
        {
            return true;
        }

        public List<string> IsGrants(int staffId, List<string> permissions)
        {
            return permissions;
        }

        public List<string> IsGrants(int staffId, List<string> permissions, string app_code)
        {
            return permissions;
        }
    }
}
