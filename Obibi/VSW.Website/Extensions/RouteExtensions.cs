using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website
{
    public static class RouteExtensions
    {
        public static string GetControllerName(this RouteData route)
        {
            return route.Values["controller"].ToString();
        }

        public static string GetControllerName(this ViewContext viewContext)
        {
            return viewContext.RouteData.GetControllerName();
        }
    }
}
