using System.Collections.Generic;
using System.IO;
using VSW.Core.Interface;
using VSW.Core.Web;

namespace VSW.Lib.Global
{
    public class IniResourceService : IResourceServiceInterface
    {
        private readonly Dictionary<string, string> _listResource;

        public IniResourceService(string fileIni)
        {
            var keyCache = Cache.CreateKey(Security.Md5(fileIni));
            var obj = Cache.GetValue(keyCache);
            if (obj != null) _listResource = (Dictionary<string, string>)obj;
            else
            {
                _listResource = new Dictionary<string, string>();
                if (System.IO.File.Exists(fileIni))
                {
                    var streamReader = new StreamReader(fileIni);
                    while (streamReader.Peek() != -1)
                    {
                        var s = streamReader.ReadLine();

                        if (s == null)
                            continue;

                        s = s.Trim();
                        if (s == string.Empty || s.StartsWith("//"))
                            continue;

                        var index = s.IndexOf('=');
                        if (index == -1)
                            continue;

                        var key = s.Substring(0, index).Trim();
                        var value = s.Substring(index + 1).Trim();

                        _listResource[key] = value;
                    }
                    streamReader.Close();
                }
                Cache.SetValue(keyCache, _listResource);
            }
        }

        public string VSW_Core_GetByCode(string code, string defalt)
        {
            return _listResource.ContainsKey(code) ? _listResource[code] : defalt;
        }
    }
}