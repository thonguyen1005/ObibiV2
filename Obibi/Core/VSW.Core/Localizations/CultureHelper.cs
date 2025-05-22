using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSW.Core
{
    public class CultureHelper
    {
        public static CultureInfo Get(CultureCode code)
        {
            return CultureInfo.GetCultureInfo((int)code);
        }

        public static string GetName(CultureCode code)
        {
            return code == CultureCode.Unknown ? null : EnumExtensions.GetName(code).Replace("_", "-");
        }

        public static CultureCode GetCode(string culture)
        {
            var name = culture.Replace("-", "_");
            CultureCode rs = CultureCode.Unknown;
            EnumExtensions.TryParse(name, out rs);
            return rs;
        }
    }
}
