namespace VSW.Website.Models.DataTables
{
    /// <summary>
    /// Represents link render for DataTables column
    /// </summary>
    public partial class RenderJavascript : IRender
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the RenderButton class 
        /// </summary>
        /// <param name="url">URL</param>
        public RenderJavascript(string function, string url)
        {
            Function = function;
            Url = url;
            Format = Format.Text;
        }
        public RenderJavascript(string function, string url, string dataId)
        {
            Function = function;
            Url = url;
            DataId = dataId;
            Format = Format.Text;
        }
        public RenderJavascript(string function, string url, string dataId, Format format)
        {
            Function = function;
            Url = url;
            DataId = dataId;
            Format = format;
        }

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets Function
        /// </summary>
        public string Function { get; set; }

        /// <summary>
        /// Gets or sets Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets data Id
        /// </summary>
        public string DataId { get; set; }

        /// <summary>
        /// Gets or sets link title 
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Format value
        /// </summary>
        public Format Format { get; set; }

        #endregion
    }
}