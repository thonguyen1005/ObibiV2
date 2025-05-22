using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website.MVC
{
    public interface IWebMenuRegister
    {
        void Register(List<WebMenuItem> menus);
    }
}
