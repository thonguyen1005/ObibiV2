using Microsoft.AspNetCore.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSW.Core;

namespace VSW.Website.MVC
{
    public class WebMenuItem
    {
        public WebMenuItem()
        {
            Visible = true;
            SubItems = new List<WebMenuItem>();
            Id = StringExtensions.NewId();
        }

        public string Id { get; set; }

        public string SystemName { get; set; }

        public bool Visible { get; set; }

        public object RouteValues { get; set; }

        public string Url { get; set; }

        public bool OpenUrlInNewTab { get; set; }

        public string Title { get; set; }

        public string ToolTip { get; set; }

        public string Image { get; set; }
        public string CssClass_Bgcolor { get; set; }

        public string CssClass { get; set; }

        public string ControllerName { get; set; }
        
        public string ActionDefault { get; set; }

        public bool ShowInHome { get; set; }

        public int Level { get; set; }

        public string PermissionCode { get; set; }

        public List<WebMenuItem> SubItems { get; set; }

        /// <summary>
        /// Lấy toàn bộ các menu con và cháu
        /// </summary>
        /// <returns></returns>
        public List<WebMenuItem> GetAllDescendantItems()
        {
            if(SubItems.IsEmpty())
            {
                return SubItems;
            }

            var lst = new List<WebMenuItem>(SubItems);
            foreach(var item in SubItems)
            {
                var temp = item.GetAllDescendantItems();
                if(temp.IsNotEmpty())
                {
                    lst.AddRange(temp);
                }    
            }

            return lst;
        }    

        public static WebMenuItem New(string title, string image, string cssClass, string controllerName, string actionDefault, RouteValueDictionary routeValues, bool showInHome, string tooltip = null)
        {
            return new WebMenuItem
            {
                CssClass = cssClass,
                Title = title,
                Image = image,
                ControllerName = controllerName,
                ActionDefault = actionDefault,
                ShowInHome = showInHome,
                ToolTip = tooltip,
                RouteValues = routeValues
            };
        }

        public void SetLevel(int level)
        {
            Level = level;
            if(SubItems.IsNotEmpty())
            {
                foreach(var sub in SubItems)
                {
                    sub.SetLevel(level + 1);
                }
            }
        }

        public WebMenuItem Clone()
        {
            var obj = new WebMenuItem
            {
                Visible = this.Visible,
                PermissionCode = this.PermissionCode,
                Level = this.Level,
                Image = this.Image,
                CssClass = this.CssClass,
                ShowInHome = this.ShowInHome,
                Url = this.Url,
                SystemName = this.SystemName,
                ActionDefault = this.ActionDefault,
                ControllerName = this.ControllerName,
                Title = this.Title,
                ToolTip = this.ToolTip,
                OpenUrlInNewTab = this.OpenUrlInNewTab,
                RouteValues = this.RouteValues
            };

            return obj;
        }
    }
}
