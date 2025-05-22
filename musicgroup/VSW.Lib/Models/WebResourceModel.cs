using System.Collections.Generic;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class WebResourceEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int LangID { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public string Value { get; set; }

        #endregion Autogen by VSW
    }

    public class WebResourceService : ServiceBase<WebResourceEntity>
    {
        #region Autogen by VSW

        public WebResourceService()
            : base("[Web_Resource]")
        {
        }

        private static WebResourceService _instance;

        public static WebResourceService Instance => _instance ?? (_instance = new WebResourceService());

        #endregion Autogen by VSW

        public WebResourceEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        public WebResourceEntity GetByCode_Cache(string code, int langID)
        {
            return CreateQuery()
               .Where(o => o.LangID == langID && o.Code == code)
               .ToSingle_Cache();
        }

        public bool CP_HasExists(string code, int langID)
        {
            return CreateQuery()
              .Where(o => o.LangID == langID && o.Code == code)
              .Count()
              .ToValue().ToBool();
        }

        public List<WebResourceEntity> GetAllByLangID_Cache(int langID)
        {
            return CreateQuery()
               .Where(o => o.LangID == langID)
               .ToList_Cache();
        }
    }
}