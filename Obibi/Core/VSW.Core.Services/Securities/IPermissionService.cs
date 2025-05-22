using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services
{
    public interface IPermissionService
    {
        bool IsGrant(int staffId, string permission);

        bool IsGrant(int staffId, string permission, string app_code);

        List<string> IsGrants(int staffId, List<string> permissions);

        List<string> IsGrants(int staffId, List<string> permissions, string app_code);
    }
}
