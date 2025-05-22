namespace VSW.Website.Models.DataTables
{
    /// <summary>
    /// Format list
    /// </summary>
    public enum Format
    {
        Text,
        Date,
        Money,
        Float
    }

    /// <summary>
    /// Represents link render for DataTables column
    /// </summary>
    public partial class RenderLink : IRender
    {
        #region Ctor

        /// <summary>
        /// Initializes a new instance of the RenderButton class 
        /// </summary>
        /// <param name="url">URL</param>
        public RenderLink(DataUrl url)
        {
            Url = url;
            Format = Format.Text;
        }
        public RenderLink(DataUrl url, Format format)
        {
            Url = url;
            Format = format;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets Url
        /// </summary>
        public DataUrl Url { get; set; }

        /// <summary>
        /// Gets or sets link title 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Format value
        /// </summary>
        public Format Format { get; set; }

        public string Target { get; set; }

        #endregion
    }
}