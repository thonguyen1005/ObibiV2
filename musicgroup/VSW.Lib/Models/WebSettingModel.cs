using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class WebSettingEntity : EntityBase
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string Code { get; set; }

        [DataInfo]
        public long Value { get; set; }

        #endregion Autogen by VSW
    }

    public class WebSettingService : ServiceBase<WebSettingEntity>
    {
        #region Autogen by VSW

        public WebSettingService()
            : base("[Web_Setting]")
        {
        }

        private static WebSettingService _instance;

        public static WebSettingService Instance => _instance ?? (_instance = new WebSettingService());

        #endregion Autogen by VSW
    }
}