using VSW.Core.Interface;
using VSW.Lib.Web;

namespace VSW.Lib.Models
{
    public class SysModuleService : IModuleServiceInterface
    {
        private static SysModuleService _instance;

        public static SysModuleService Instance => _instance ?? (_instance = new SysModuleService());

        public IModuleInterface VSW_Core_GetByCode(string code)
        {
            return Application.Modules.Find(o => o.Code == code);
        }
    }
}