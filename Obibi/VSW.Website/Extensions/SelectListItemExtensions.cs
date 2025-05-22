using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSW.Core;
using VSW.Core.Services;

namespace VSW.Website
{
    public static class SelectListItemExtensions
    {
        public const string ALL_NAME = "--- Tất cả ---";
        public const string NON_SELECT_NAME = "--- Chọn ---";
        public const string ITEM_TEMPLATE = "--- {0} ---";

        public static void AddSelectAllItem(this IList<SelectListItem> items, object value)
        {
            items.Add(new SelectListItem(ALL_NAME, value.ToString()));
        }

        public static void AddNoneSelectItem(this IList<SelectListItem> items, object value)
        {
            items.Add(new SelectListItem(NON_SELECT_NAME, value.ToString()));
        }

        public static void PrepareDefaultItem(this IList<SelectListItem> items, bool withSpecialDefaultItem, string defaultItemText = "", string defaultItemValue = "")
        {
            if (items == null)
                items = new List<SelectListItem>();

            //whether to insert the first special item for the default value
            if (!withSpecialDefaultItem)
                return;

            //prepare item text
            defaultItemText ??= NON_SELECT_NAME;
            defaultItemValue ??= "";
            //insert this default item at first
            items.Insert(0, new SelectListItem { Text = defaultItemText, Value = defaultItemValue });
        }
        public static void PrepareData(
            this List<SelectListItem> items
            , object data
            , string column_name
            , string column_value
            , bool withSpecialDefaultItem = false
            , string defaultItemText = null
            , string defaultItemValue = null)
        {
            if (items == null)
                items = new List<SelectListItem>();
            if (data != null)
            {
                try
                {
                    JToken objs = data.ToJToken();
                    if (objs != null)
                    {
                        foreach (var item in objs)
                        {
                            string name = item[column_name].ToString();
                            string value = item[column_value].ToString();
                            items.Add(new SelectListItem { Value = value, Text = name });
                        }
                    }
                }
                catch { }
            }
            items.PrepareDefaultItem(withSpecialDefaultItem, defaultItemText, defaultItemValue);
        }

    }
}
