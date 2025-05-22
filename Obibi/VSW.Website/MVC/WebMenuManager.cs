using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using VSW.Core;
using VSW.Core.Services;

namespace VSW.Website.MVC
{
    public static class WebMenuManager
    {
        private readonly static List<WebMenuItem> _menus = new List<WebMenuItem>();

        public static List<WebMenuItem> Menus
        {
            get { return _menus; }
        }

        public static string GetPermissionCode<TController>(Expression<Func<TController, object>> expAction)
        {
            var memberInfo = (expAction.Body as MethodCallExpression).Method;
            var att = memberInfo.GetCustomAttributes(true).Where(x => x is PermissionAttribute).Select(x => x as PermissionAttribute).FirstOrDefault();
            if (att != null)
            {
                return att.Code;
            }

            att = typeof(TController).GetCustomAttributes(true).Where(x => x is PermissionAttribute).Select(x => x as PermissionAttribute).FirstOrDefault();
            return att == null ? "" : att.Code;
        }

        public static WebMenuItem WithController<TController>(this WebMenuItem item, Expression<Func<TController, object>> expAction = null)
        {
            item.ControllerName = GetControllerName(typeof(TController).Name);
            if(expAction != null)
            {
                var memberInfo = (expAction.Body as MethodCallExpression).Method;
                item.ActionDefault = memberInfo.Name;
                item.PermissionCode = GetPermissionCode(expAction);
            }                
            return item;
        }


        public static string GetControllerName(string controllerName)
        {
            return controllerName.Substring(0, controllerName.Length - "controller".Length);
        }

        private static List<string> GetGrantedMenuIds(List<WebMenuItem> lst, List<string> lstPermission)
        {
            var lstRs = new List<string>();
            foreach (var obj in lst)
            {
                bool hasPermission = true;
                if(obj.PermissionCode.IsNotEmpty())
                { 
                    if(!lstPermission.Contains(obj.PermissionCode))
                    {
                        hasPermission = false;
                    }    
                }    

                if(obj.SubItems.IsNotEmpty())
                {
                    var lstChild = GetGrantedMenuIds(obj.SubItems, lstPermission);
                    if(lstChild.IsEmpty())
                    {
                        hasPermission = false;
                    }    
                    else
                    {
                        lstRs.AddRange(lstChild);
                    }    
                }   
                
                if(hasPermission)
                {
                    lstRs.Add(obj.Id);
                }    
            }

            return lstRs;
        }


        public static void Init()
        {
            if (_menus.IsEmpty())
            {
                var register = CoreService.ServiceProvider.GetService<IWebMenuRegister>();
                register.Register(_menus);
            }
        }

        private static bool _initedUrl = false;
        private static void InitUrl(this IUrlHelper urlHelper)
        {
            if (_initedUrl)
            {
                return;
            }

            _menus.ForEach(x => urlHelper.InitUrl(x));
            _initedUrl = true;
        }

        private static void InitUrl(this IUrlHelper urlHelper, WebMenuItem item)
        {
            if (item.Url.IsNotEmpty() || item.SubItems.IsNotEmpty())
            {
                return;
            }

            var url = urlHelper.Action(item.ActionDefault.IsEmpty() ? "Index" : item.ActionDefault, item.ControllerName, item.RouteValues);
            item.Url = url;
        }

        public static WebMenuItem GetCurrentMenu(this IUrlHelper urlHelper, string currentUrl)
        {
            InitUrl(urlHelper);

            return _menus.Where(x => x.Url.IsNotEmpty() && x.Url.NotEqualsIgnoreCase("/") && (x.Url.EqualsIgnoreCase(currentUrl) || currentUrl.StartsWith(x.Url, StringComparison.OrdinalIgnoreCase))).OrderByDescending(x => x.Url).FirstOrDefault();
        }

        public static string GetMenuUrl(this IUrlHelper helper, WebMenuItem item)
        {
            InitUrl(helper);
            return item.Url;
        }

        public static bool IsActive(this WebMenuItem item, string currentUrl)
        {
            return item.Url.IsNotEmpty() && item.Url.EqualsIgnoreCase(currentUrl);
        }
    }
}
