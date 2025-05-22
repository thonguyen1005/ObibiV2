using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Messages
{
    /// <summary>
    /// Represents default values related to messages services
    /// </summary>
    public static partial class MessageDefaults
    {
        /// <summary>
        /// Gets a key for notifications list from TempDataDictionary
        /// </summary>
        public static string NotificationListKey => "NotificationList";
        public static string NotificationAlertKey => "NotificationAlert";
    }
}
