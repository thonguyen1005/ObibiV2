using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class WebRedirectionEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public string Url { get; set; }

        [DataInfo]
        public string Redirect { get; set; }

        #endregion Autogen by VSW
    }

    public class WebRedirectionService : ServiceBase<WebRedirectionEntity>
    {
        #region Autogen by VSW

        public WebRedirectionService()
            : base("[Web_Redirection]")
        {
        }

        private static WebRedirectionService _instance;

        public static WebRedirectionService Instance
        {
            get { return _instance ?? (_instance = new WebRedirectionService()); }
        }

        #endregion Autogen by VSW

        public WebRedirectionEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }
    }
}