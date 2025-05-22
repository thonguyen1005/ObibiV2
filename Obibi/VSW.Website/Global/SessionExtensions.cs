using VSW.Core;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;

namespace VSW.Website.Global
{
    public static class SessionExtensions
    {
        public static int GetUnitId(this IWebSession session)
        {
            var rs = session.GetInt32("Unit");
            return rs.HasValue ? rs.Value : 0;
        }
        public static int GetStaffId(this IWebSession session)
        {
            var rs = session.GetInt32("Staff");
            return rs.HasValue ? rs.Value : 0;
        }

        public static string GetStaffName(this IWebSession session)
        {
            return session.GetString("StaffName");
        }
        public static string GetUnitName(this IWebSession session)
        {
            return session.GetString("UnitName");
        }

        public static void SetUnit(this IWebSession session, int unitId ,string unitName)
        {
            session.SetInt32("Unit", unitId);
            session.SetString("UnitName", unitName);
        }
        public static void SetStaff(this IWebSession session, int staffId, string staffName)
        {
            session.SetInt32("Staff", staffId);
            session.SetString("StaffName", staffName);
        }

    }
}

