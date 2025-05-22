using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website.MVC
{
    public static class WebMenuItemExtensions
    {
        /// <summary>
        /// Checks whether this menu item or child ones has a specified system name
        /// </summary>
        /// <param name="item">MenuItem</param>
        /// <param name="systemName">System name</param>
        /// <returns>Result</returns>
        public static bool ContainsSystemName(this WebMenuItem item, string systemName)
        {
            if (item == null)
                throw new ArgumentNullException(nameof(item));

            if (string.IsNullOrWhiteSpace(systemName))
                return false;

            if (systemName.Equals(item.SystemName, StringComparison.InvariantCultureIgnoreCase))
                return true;

            return item.SubItems.Any(si => ContainsSystemName(si, systemName));
        }
    }
}
