using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VSW.Website.Models
{
    /// <summary>
    /// Represents a paging request model
    /// </summary>
    public partial interface IPagingRequestModel
    {
        /// <summary>
        /// Gets a page number
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// Gets a page size
        /// </summary>
        int PageSize { get; }
    }
}
