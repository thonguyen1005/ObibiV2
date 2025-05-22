using System.Collections.Generic;
using System.Web;
using VSW.Core.Interface;
using VSW.Core.Web;
using VSW.Lib.Models;

namespace VSW.Lib.Global
{
    public class IniSqlResourceService : IResourceServiceInterface
    {
        private readonly string _langCode;
        private readonly Dictionary<string, string> _listResource;

        public IniSqlResourceService(ILangInterface item)
        {
            _langCode = item.Code;

            var keyCache = Cache.CreateKey(Security.Md5(item.ID + "." + item.Code));
            var obj = Cache.GetValue(keyCache);
            if (obj != null)
                _listResource = (Dictionary<string, string>)obj;
            else
            {
                _listResource = new Dictionary<string, string>();

                var listSqlResource = WebResourceService.Instance.GetAllByLangID_Cache(item.ID);
                for (var i = 0; listSqlResource != null && i < listSqlResource.Count; i++)
                {
                    _listResource[listSqlResource[i].Code] = listSqlResource[i].Value;
                }

                Cache.SetValue(keyCache, _listResource);
            }
        }

        private IniResourceService _iniResourceService;

        public string VSW_Core_GetByCode(string code, string defalt)
        {
            if (_listResource.ContainsKey(code))
                return _listResource[code];

            if (_iniResourceService == null)
                _iniResourceService = new IniResourceService(HttpContext.Current.Server.MapPath("~/Views/Lang/" + _langCode + ".ini"));

            return _iniResourceService.VSW_Core_GetByCode(code, defalt);
        }
    }
}