using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using VSW.Core;

namespace VSW.Website.Extensions
{
    public static class Extensions
    {
        public static SelectList SelectListFor<T>() where T : struct
        {
            var t = typeof(T);

            if (!t.IsEnum)
            {
                return null;
            }

            var values = Enum.GetValues(typeof(T)).Cast<T>()
                           .Select(e => new { Id = Convert.ToInt32(e), Name = (e as Enum).GetDescription() });

            return new SelectList(values, "Id", "Name");
        }

        public static TAttribute GetAttribute<TAttribute>(this Enum enumValue)
                where TAttribute : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<TAttribute>();
        }
    }


}
