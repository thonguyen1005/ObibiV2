using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace VSW.Website.Models
{
    /// <summary>
    /// Represents the base paged list model (implementation for DataTables grids)
    /// </summary>
    public abstract partial class BasePagedListModel<T> : BaseModel, IPagedModel<T>
    {
        /// <summary>
        /// Gets or sets data records
        /// </summary>
        [JsonPropertyName("data")]
        public IEnumerable<T> Data { get; set; }

        /// <summary>
        /// Gets or sets draw
        /// </summary>
        [JsonPropertyName("draw")]
        public string Draw { get; set; }

        /// <summary>
        /// Gets or sets a number of filtered data records
        /// </summary>
        [JsonPropertyName("recordsFiltered")]
        public int RecordsFiltered { get; set; }

        /// <summary>
        /// Gets or sets a number of total data records
        /// </summary>
        [JsonPropertyName("recordsTotal")]
        public int RecordsTotal { get; set; }

        //TODO: remove after moving to DataTables grids
        [JsonPropertyName("total")]
        public int Total { get; set; }
    }
}