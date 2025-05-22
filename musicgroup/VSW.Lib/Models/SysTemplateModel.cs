using VSW.Core.Interface;
using VSW.Core.Models;

namespace VSW.Lib.Models
{
    public class SysTemplateEntity : EntityBase, ITemplateInterface
    {
        #region Autogen by VSW

        [DataInfo]
        public override int ID { get; set; }

        [DataInfo]
        public int LangID { get; set; }

        [DataInfo]
        public int Device { get; set; }

        [DataInfo]
        public override string Name { get; set; }

        [DataInfo]
        public string File { get; set; }

        [DataInfo]
        public string Custom { get; set; }

        [DataInfo]
        public int Order { get; set; }

        #endregion Autogen by VSW

        private string _oDeviceText;

        public string DeviceText
        {
            get
            {
                if (string.IsNullOrEmpty(_oDeviceText))
                    _oDeviceText = Device == 0 ? "PC" : (Device == 1 ? "Mobile" : "Tablet");

                return _oDeviceText;
            }
        }
    }

    public class SysTemplateService : ServiceBase<SysTemplateEntity>, ITemplateServiceInterface
    {
        #region Autogen by VSW

        public SysTemplateService() : base("[Sys_Template]")
        {
        }

        private static SysTemplateService _instance;

        public static SysTemplateService Instance => _instance ?? (_instance = new SysTemplateService());

        #endregion Autogen by VSW

        public SysTemplateEntity GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle();
        }

        #region ITemplateServiceInterface Members

        public ITemplateInterface VSW_Core_GetByID(int id)
        {
            return CreateQuery()
               .Where(o => o.ID == id)
               .ToSingle_Cache();
        }

        public void VSW_Core_CPSave(ITemplateInterface item)
        {
            Save(item as SysTemplateEntity);
        }

        #endregion ITemplateServiceInterface Members
    }
}