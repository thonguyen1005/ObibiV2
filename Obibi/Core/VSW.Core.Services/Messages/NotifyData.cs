using System;
using System.Collections.Generic;
using System.Text;

namespace VSW.Core.Services.Messages
{
    public enum NotifyType
    {
        /// <summary>
        /// Success
        /// </summary>
        Success,

        /// <summary>
        /// Error
        /// </summary>
        Error,

        /// <summary>
        /// Warning
        /// </summary>
        Warning
    }

    /// <summary>
    /// Notify Data Messages
    /// </summary>
    public struct NotifyData
    {
        /// <summary>
        /// Message type (success/warning/error)
        /// </summary>
        public NotifyType Type { get; set; }

        /// <summary>
        /// Message text
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get a sets a value indicating whether the message should not be HTML encoded
        /// </summary>
        public bool Encode { get; set; }
    }
}
