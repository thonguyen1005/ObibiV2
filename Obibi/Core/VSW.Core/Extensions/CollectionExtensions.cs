using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VSW.Core
{
    public static class CollectionExtensions
    {
        public static bool IsEmpty(this ICollection data)
        {
            return data == null || data.Count == 0;
        }

        public static bool IsNotEmpty(this ICollection data)
        {
            return !data.IsEmpty();
        }


        public static string Join(this IEnumerable<object> array, string token = ",")
        {
            if (array == null)
            {
                return null;
            }

            var lst = array.ToArray();

            return string.Join(token, lst);
        }
    }
}